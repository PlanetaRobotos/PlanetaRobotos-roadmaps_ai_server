using System.Reflection;
using Fleet.Application.Behaviors;
using Fleet.Application.Core;
using Fleet.Application.Services;
using FluentValidation;
using Mapster;
using Mediator;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Fleet.Application;

public static class DependencyInjection
{
    public static void AddApplication(this IServiceCollection services)
    {
        var configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();

        services.AddSingleton(TimeProvider.System);
        services.AddMediator(options => options.ServiceLifetime = ServiceLifetime.Scoped);
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        var appAssembly = typeof(DependencyInjection).Assembly;
        services.AddConfigurableOptions(configuration, appAssembly);
        services.AddValidatorsFromAssembly(appAssembly);
        services.AddModelValidators(appAssembly);
        services.AddCustomMappings(appAssembly);
    }


    private static void AddModelValidators(this IServiceCollection services, Assembly assembly)
    {
        var validatableTypes = assembly.GetTypes()
            .Where(t => t.GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IValidatable<>)))
            .ToArray();

        foreach (var type in validatableTypes)
        {
            var interfaceType = type.GetInterfaces()
                .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IValidatable<>));
            var modelType = interfaceType.GetGenericArguments()[0];
            var validatorType = typeof(IValidator<>).MakeGenericType(modelType);

            services.AddTransient(validatorType, _ =>
            {
                var validatableInstance = Activator.CreateInstance(type);
                var validator = (IValidator)Activator.CreateInstance(typeof(InlineValidator<>).MakeGenericType(modelType))!;
                var configureValidatorMethod = interfaceType.GetMethod("ConfigureValidator");
                configureValidatorMethod!.Invoke(validatableInstance, [validator]);
                return validator;
            });
        }
    }

    private static void AddConfigurableOptions(this IServiceCollection services, IConfiguration configuration, Assembly assembly)
    {
        // Get all types in the current assembly (or specify a different assembly if needed)
        var types = assembly.GetTypes();

        // Filter out the types that implement IConfigureOptions<>
        var configureOptionsTypes = types.Where(t => t.GetInterfaces().Any(i =>
            i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IConfigureOptions<>)));

        foreach (var configType in configureOptionsTypes)
        {
            // Get the IConfigureOptions<> interface for the current type
            var configInterface = configType.GetInterfaces().FirstOrDefault(i =>
                i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IConfigureOptions<>));

            if (configInterface == null)
                continue;

            // Register the configuration class with the DI container
            services.AddSingleton(configInterface, configType);

            // Optionally, you can instantiate the configuration class to access any properties or methods if needed
            var constructor = configType.GetConstructors().FirstOrDefault();
            if (constructor == null)
                continue;

            var parameters = constructor.GetParameters()
                .Select(p => p.ParameterType == typeof(IConfiguration) ? configuration as object : null)
                .ToArray();

            Activator.CreateInstance(configType, parameters);
        }
    }


    private static void AddCustomMappings(this IServiceCollection services, Assembly assembly)
    {
        services.AddSingleton(TypeAdapterConfig.GlobalSettings);
        TypeAdapterConfig.GlobalSettings.Scan(assembly);

        var mappableTypes = assembly.GetTypes()
            .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICustomMappable<,>)))
            .ToList();

        foreach (var type in mappableTypes)
        {
            var interfaceType = type.GetInterfaces()
                .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICustomMappable<,>));

            var modelType = interfaceType.GetGenericArguments()[0];
            var targetType = interfaceType.GetGenericArguments()[1];

            // Create an instance of the mappable type
            var mappableInstance = Activator.CreateInstance(type);

            // Create TypeAdapterSetter instances using reflection
            var typeAdapterSetterModelToTarget = typeof(TypeAdapterConfig<,>)
                .MakeGenericType(modelType, targetType)
                .GetMethod("NewConfig", BindingFlags.Static | BindingFlags.Public)!
                .Invoke(null, null);

            var typeAdapterSetterTargetToModel = typeof(TypeAdapterConfig<,>)
                .MakeGenericType(targetType, modelType)
                .GetMethod("NewConfig", BindingFlags.Static | BindingFlags.Public)!
                .Invoke(null, null);

            // Configure mappings using reflection
            var configureMapperMethod = interfaceType.GetMethod("ConfigureMapper");
            configureMapperMethod!.Invoke(mappableInstance, [typeAdapterSetterModelToTarget]);

            var configureInvertMapperMethod = interfaceType.GetMethod("ConfigureInvertMapper");
            configureInvertMapperMethod!.Invoke(mappableInstance, [typeAdapterSetterTargetToModel]);
        }
    }
}