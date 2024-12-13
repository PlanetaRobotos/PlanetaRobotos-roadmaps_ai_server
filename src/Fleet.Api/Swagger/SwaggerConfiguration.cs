using Asp.Versioning.ApiExplorer;
using Fleet.Api.Swagger.Filters;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Fleet.Api.Swagger;

internal sealed class SwaggerConfiguration(
    IWebHostEnvironment environment,
    IApiVersionDescriptionProvider provider,
    IOptions<SwaggerConfigurationOptions> swaggerOptionsAccessor
) : IConfigureNamedOptions<SwaggerGenOptions>
{
    private readonly SwaggerConfigurationOptions _config = swaggerOptionsAccessor.Value;

    public void Configure(SwaggerGenOptions options)
    {
        // Add swagger document for every API version discovered
        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName.ToLower(), CreateVersionInfo(description));
        }

        options.SchemaFilter<BodyOrRouteSchemaFilter>();

        options.DocumentFilter<JsonPatchDocumentFilter>();

        options.ParameterFilter<CamelCaseQueryParameterFilter>();

        options.SupportNonNullableReferenceTypes();

        if (_config.Security.Enabled)
        {
            options.OperationFilter<AuthorizeCheckOperationFilter>();

            options.AddSecurityDefinition(_config.Security.Scheme,
                new OpenApiSecurityScheme
                {
                    Description = _config.Security.Description,
                    Name = _config.Security.ParameterName,
                    In = _config.Security.ParameterLocation,
                    Type = _config.Security.SchemeType,
                    BearerFormat = _config.Security.BearerFormat,
                    Scheme = _config.Security.Scheme
                });
        }

        // Custom operation ID generator
        options.CustomOperationIds(apiDesc =>
            $"{apiDesc.ActionDescriptor.RouteValues["controller"]}_{(apiDesc.ActionDescriptor.RouteValues["action"] ?? "unknownAction")}");

        foreach (string xmlPath in GetXmlComments())
        {
            options.IncludeXmlComments(xmlPath, includeControllerXmlComments: true);
        }
    }

    public void Configure(string? name, SwaggerGenOptions options) => Configure(options);


    private OpenApiInfo CreateVersionInfo(ApiVersionDescription versionDescription)
    {
        string descriptionFilePath = Path.Join(environment.ContentRootPath, _config.Description);
        string description = File.Exists(descriptionFilePath)
            ? File.ReadAllText(descriptionFilePath)
            : _config.Description ?? default!;

        if (!string.IsNullOrWhiteSpace(description))
        {
            description = description.Replace("{version}", versionDescription.GroupName.ToLower());
        }

        OpenApiInfo openApiInfo = new OpenApiInfo();
        openApiInfo.Version ??= versionDescription.ApiVersion.ToString();
        openApiInfo.Description ??= description;
        openApiInfo.Title ??= _config.Title;
        return openApiInfo;
    }

    private IEnumerable<string> GetXmlComments()
    {
        // Set the comments path for the Swagger JSON and UI
        if (_config.IncludeComments is not { Length: > 0 })
            yield break;

        foreach (string xmlDocsFile in _config.IncludeComments)
        {
            string xmlDocPath = Path.Combine(AppContext.BaseDirectory, xmlDocsFile);
            if (File.Exists(xmlDocPath))
                yield return xmlDocPath;
        }
    }
}