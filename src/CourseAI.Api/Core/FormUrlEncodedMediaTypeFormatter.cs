using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;

namespace CourseAI.Api.Core;

public class FormUrlEncodedMediaTypeFormatter : InputFormatter
{
    public FormUrlEncodedMediaTypeFormatter()
    {
        SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded"));
    }

    public override bool CanRead(InputFormatterContext context)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));
        
        var contentType = context.HttpContext.Request.ContentType;
        return contentType != null && contentType.StartsWith("application/x-www-form-urlencoded", StringComparison.OrdinalIgnoreCase);
    }

    public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
    {
        var request = context.HttpContext.Request;
        var formCollection = await request.ReadFormAsync();
        
        var type = context.ModelType;
        var model = Activator.CreateInstance(type);
        
        foreach (var property in type.GetProperties())
        {
            if (formCollection.TryGetValue(property.Name, out var value))
            {
                var convertedValue = Convert.ChangeType(value.ToString(), property.PropertyType);
                property.SetValue(model, convertedValue);
            }
        }

        return await InputFormatterResult.SuccessAsync(model);
    }
}