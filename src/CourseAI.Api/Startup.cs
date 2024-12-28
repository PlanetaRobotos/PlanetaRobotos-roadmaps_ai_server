using System.Net;
using System.Net.Mail;
using CourseAI.Api.Extensions;
using CourseAI.Api.Middlewares;
using CourseAI.Api.Swagger.Extensions;
using CourseAI.Application;
using CourseAI.Application.Extensions;
using CourseAI.Application.Options;
using CourseAI.Domain;
using CourseAI.Infrastructure;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace CourseAI.Api;

internal static class Startup
{
    public static void ConfigureBuilder(WebApplicationBuilder builder)
    {
        builder.Logging.AddConsole();

        builder.Services.AddApplicationInsightsTelemetry();
        // builder.Services.AddSwaggerGenWithAuth();
        builder.Services.AddCustomSwagger();

        builder.Configuration.AddJsonFile("appsettings.Secrets.json", optional: true);
        builder.Host.UseSerilog();

        builder.Services.AddHttpContextAccessor();
        builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
        builder.Services.AddScoped<IUrlHelper>(sp =>
        {
            var actionContext = sp.GetRequiredService<IActionContextAccessor>().ActionContext;
            var factory = sp.GetRequiredService<IUrlHelperFactory>();
            return factory.GetUrlHelper(actionContext);
        });

        builder.Services.AddDomain();
        builder.Services.AddApplication();
        builder.Services.AddInfrastructure();
        builder.Services.AddWebApi();

        var accessToken = builder.Services.GetOptions<JwtOptions>().Value.AccessToken;
        var emailOptions = builder.Services.GetOptions<EmailOptions>().Value;
        
        var smtpClient = new SmtpClient
        {
            Port = emailOptions.Port,
            Credentials = new NetworkCredential(emailOptions.Username, emailOptions.Password),
            Host = emailOptions.Host,
            Timeout = 10000,
            EnableSsl = emailOptions.EnableSsl
        };

        builder.Services
            .AddFluentEmail(emailOptions.SenderEmail, emailOptions.Sender)
            .AddSmtpSender(smtpClient);
        
        builder.Services.AddAuthorization();
        builder.Services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(jwt =>
            {
                jwt.RequireHttpsMetadata = false;
                jwt.SaveToken = true;

                jwt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = accessToken.Issuer is not null,
                    ValidateAudience = accessToken.Audiences is not null,
                    ValidateIssuerSigningKey = true,
                    // // RequireExpirationTime = true,
                    // // ValidateLifetime = true,
                    IssuerSigningKey = accessToken.Secret,
                    ValidIssuer = accessToken.Issuer,
                    ValidAudiences = accessToken.Audiences
                    // ClockSkew = TimeSpan.Zero
                };

                jwt.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                        logger.LogError($"OnAuthenticationFailed {context.Exception.Message}");
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = context =>
                    {
                        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                        logger.LogInformation("Token successfully validated.");
                        return Task.CompletedTask;
                    }
                };
            }).AddCookie()
            .AddGoogle(options =>
            {
                options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
                options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
                options.CallbackPath = "/external-login/google-callback";
                options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;

                options.Events = new OAuthEvents
                {
                    OnRemoteFailure = context =>
                    {
                        var error = context.Failure;
                        var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();

                        logger.LogError($"Failed to authenticate user {error.Message}");
                        return Task.CompletedTask;
                    }
                };
            });
    }

    public static void ConfigureWebApp(WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            // app.UseSwagger();
            // app.UseSwaggerUI();

            app.ApplyMigrations();
        }

        var devOptions = app.Services.GetOptions<DevelopmentOptions>().Value;
        // if (devOptions.EnableSwagger)
        {
            app.UseCustomSwagger();
            // app.UseSwagger();
            // app.UseSwaggerUI(c =>
            // {
            //     c.SwaggerEndpoint("/swagger/v1/swagger.json", "Fleer API v1");
            // });
            // app.ForceRedirect(from: "/", to: "/swagger/index.html");
        }

        app.UseCors("AcceptAll");

        if (app.Environment.IsProduction())
        {
            // app.UseResponseCaching();
            app.UseHttpsRedirection();
        }

        app.UseHttpsRedirection();

        // app.MapIdentityApi<User>();

        app.UseSerilogRequestLogging();

        app.UseMiddleware<CustomExceptionHandler>();

        // app.MapIdentityApiWithCustomRoutes<User>();

        app.UseMiddleware<TokenLoggingMiddleware>();

        app.UseAuthentication();
        // app.UseRouting();
        app.UseAuthorization();
        // app.UseEndpoints(endpoints =>
        // {
        //     endpoints.MapControllers();
        // });
        app.MapControllers();

        if (app.Environment.IsProduction())
        {
            app.MapGet("/", () => "OK");
        }
    }
}