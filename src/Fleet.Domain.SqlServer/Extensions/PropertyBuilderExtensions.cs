using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Fleet.Domain.Extensions;

public static class PropertyBuilderExtensions
{
    public static PropertyBuilder<TProperty> HasSequentialGuidAsDefault<TProperty>(this PropertyBuilder<TProperty> builder)
    {
        return builder.HasDefaultValueSql("NEWSEQUENTIALID()");
    }


    public static PropertyBuilder<TProperty> HasEnumToStringConversion<TProperty>(this PropertyBuilder<TProperty> propertyBuilder)
    {
        var propertyInfo = propertyBuilder.Metadata.PropertyInfo!;
        var enumType = propertyInfo.PropertyType;

        if (!enumType.IsEnum)
        {
            throw new ArgumentException("Property type must be an enum");
        }

        var converterType = typeof(EnumToStringConverter<>).MakeGenericType(enumType);
        var converterInstance = Activator.CreateInstance(converterType);

        var hasConversionMethod = typeof(PropertyBuilder)
            .GetMethods()
            .First(m => m.Name == "HasConversion"
                        && m.GetParameters().Length == 1
                        && m.GetParameters()[0].ParameterType == typeof(ValueConverter));

        hasConversionMethod.Invoke(propertyBuilder, [converterInstance]);

        return propertyBuilder;
    }
}