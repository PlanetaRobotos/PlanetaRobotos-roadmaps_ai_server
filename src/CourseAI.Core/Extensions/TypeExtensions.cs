using System.Collections;
using System.Reflection;

namespace CourseAI.Core.Extensions;

public static class TypeExtensions
{
    public static PropertyInfo[] GetPropertiesExcluding<TInterface>(this Type targetType)
    {
        return targetType.GetProperties()
            .Where(prop => !typeof(TInterface).IsAssignableFrom(prop.PropertyType))
            .ToArray();
    }

    public static PropertyInfo[] GetPropertiesExcluding(this Type targetType, params Type[] interfaceTypes)
    {
        return targetType.GetProperties()
            .Where(prop => !interfaceTypes.All(i => i.IsAssignableFrom(prop.PropertyType)))
            .ToArray();
    }


    public static IEnumerable<PropertyInfo> GetNestedPropertiesWith<TAttribute>(this Type type, bool includeInherited = true)
        where TAttribute : Attribute
    {
        var properties = new List<PropertyInfo>();
        type.ScanPropertiesRecursively<TAttribute>("", properties, [], includeInherited);
        return properties;
    }

    private static void ScanPropertiesRecursively<TAttribute>(
        this Type type, string propertyPath, List<PropertyInfo> result, HashSet<Type> visitedTypes, bool includeInherited)
        where TAttribute : Attribute
    {
        // Prevent circular references
        if (!visitedTypes.Add(type))
            return;

        var bindingFlags = BindingFlags.Public | BindingFlags.Instance;
        if (includeInherited)
            bindingFlags |= BindingFlags.FlattenHierarchy;

        var properties = type.GetProperties(bindingFlags);

        foreach (var property in properties)
        {
            // Skip indexer properties
            if (property.GetIndexParameters().Length > 0)
                continue;

            // Check if property has the attribute
            if (property.GetCustomAttribute<TAttribute>() != null)
            {
                result.Add(property);
            }

            // If property type is a class (not primitive or collection), scan it recursively
            if (property.PropertyType.ShouldScanPropertyType())
            {
                ScanPropertiesRecursively<TAttribute>(
                    property.PropertyType,
                    $"{propertyPath}{property.Name}.",
                    result,
                    [..visitedTypes],
                    includeInherited);
            }
        }
    }

    private static bool ShouldScanPropertyType(this Type? type)
    {
        if (type is null) return false;
        return type.IsClass
               && type != typeof(string)
               && !type.IsArray
               && !typeof(IEnumerable).IsAssignableFrom(type)
               && !type.IsGenericType;
    }

    public static string GetPropertyPath(this PropertyInfo property)
    {
        var declaringType = property.DeclaringType;
        var path = new Stack<string>();
        path.Push(property.Name.ToCamelCase());

        while (declaringType != null)
        {
            if (declaringType.DeclaringType != null)
                path.Push(declaringType.Name);
            declaringType = declaringType.DeclaringType;
        }

        return string.Join(".", path);
    }
}