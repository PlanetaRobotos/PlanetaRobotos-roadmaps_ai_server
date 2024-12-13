using Microsoft.OpenApi.Models;

namespace Fleet.Api.Swagger;

/// <summary>
///
/// </summary>
public sealed class SwaggerConfigurationOptions
{
    /// <summary>
    ///   Disables if <b>false</b> (enabled by default)
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    ///   Swagger built-in security options
    /// </summary>
    public SwaggerSecurityOptions Security { get; set; } = new();

    /// <summary>
    ///   Custom Swagger Docs title 
    /// </summary>
    public string? Title { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string[]? IncludeComments { get; set; }
}

/// <summary>
///
/// </summary>
public sealed class SwaggerSecurityOptions
{
    public bool Enabled { get; set; } = false;
    public string Scheme { get; set; } = "Bearer";
    public SecuritySchemeType SchemeType { get; set; } = SecuritySchemeType.ApiKey;
    public string BearerFormat { get; set; } = "JWT";
    public string ParameterName { get; set; } = "Authorization";
    public ParameterLocation ParameterLocation { get; set; } = ParameterLocation.Header;
    public string Description { get; set; } = "";
}