using System.Globalization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CourseAI.Api.Swagger.Filters;

public class CamelCaseQueryParameterFilter : IParameterFilter
{
    public void Apply(OpenApiParameter parameter, ParameterFilterContext context)
    {
        if (parameter.In == ParameterLocation.Query)
        {
            parameter.Name = ToCamelCase(parameter.Name);
        }
    }

    private string ToCamelCase(string input)
    {
        if (string.IsNullOrEmpty(input) || !char.IsUpper(input[0]))
            return input;

        var chars = input.ToCharArray();
        chars[0] = char.ToLower(chars[0], CultureInfo.InvariantCulture);
        return new string(chars);
    }
}