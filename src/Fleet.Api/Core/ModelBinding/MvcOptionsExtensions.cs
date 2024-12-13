using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace Fleet.Api.Core.ModelBinding;

public static class MvcOptionsExtensions
{
    public static void AddFromBodyOrRouteModelBinder(this MvcOptions options)
    {
        var bodyModelBinder = options.GetBodyModelBinder();
        if (GetReaderFactory(bodyModelBinder) is IHttpRequestStreamReaderFactory readerFactory)
            options.AddFromBodyOrRouteModelBinder(readerFactory);

        throw new InvalidOperationException(
            $"{nameof(BodyModelBinderProvider)} has unexpected state to register {nameof(FromBodyOrRouteModelBinderProvider)}");
    }

    public static void AddFromBodyOrRouteModelBinder(this MvcOptions options, IServiceCollection services)
    {
        var provider = services.BuildServiceProvider();
        options.AddFromBodyOrRouteModelBinder(provider);
    }

    public static void AddFromBodyOrRouteModelBinder(this MvcOptions options, IServiceProvider provider)
    {
        var loggerFactory = provider.GetRequiredService<ILoggerFactory>();
        var readerFactory = provider.GetRequiredService<IHttpRequestStreamReaderFactory>();
        options.AddFromBodyOrRouteModelBinder(readerFactory, loggerFactory);
    }


    private static void AddFromBodyOrRouteModelBinder(
        this MvcOptions options, IHttpRequestStreamReaderFactory readerFactory, ILoggerFactory? loggerFactory = null)
    {
        options.ModelBinderProviders.Insert(0,
            new FromBodyOrRouteModelBinderProvider(options.InputFormatters, readerFactory, loggerFactory, options));

        var bodyModelBinder = options.GetBodyModelBinder();
        if (bodyModelBinder is not null)
            options.ModelBinderProviders.Remove(bodyModelBinder);
    }

    private static IModelBinderProvider? GetBodyModelBinder(this MvcOptions options) =>
        options.ModelBinderProviders.FirstOrDefault(p => p is BodyModelBinderProvider);

    private static object? GetReaderFactory(IModelBinderProvider? bodyModelBinder) => typeof(BodyModelBinderProvider)
        .GetField("_readerFactory", BindingFlags.Instance | BindingFlags.NonPublic)?
        .GetValue(bodyModelBinder);
}