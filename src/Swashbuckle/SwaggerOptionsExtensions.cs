using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Microsoft.Extensions.DependencyInjection;

public static class SwaggerOptionsExtensions
{
    private static string[] Scopes;

    public static void AddIdentityServer(this SwaggerGenOptions options, string authority)
    {
        var authoritUrl = new Uri(authority);

        AddOAuth2(options, new Uri(authoritUrl, "connect/authorize"), new Uri(authoritUrl, "connect/token"));
    }

    public static void AddOAuth2(this SwaggerGenOptions options, Uri authorizationUrl, Uri tokenUrl)
    {
        options.AddSecurityDefinition("oAuth2", new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.OAuth2,
            Flows = new OpenApiOAuthFlows
            {
                AuthorizationCode = new OpenApiOAuthFlow
                {
                    AuthorizationUrl = authorizationUrl,
                    TokenUrl = tokenUrl,
                    Scopes = Scopes?.ToDictionary(item => item, item => item),
                }
            }
        });

        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Id = "oAuth2",
                        Type = ReferenceType.SecurityScheme
                    }
                },
                Array.Empty<string>()
            }
        });

        options.OperationFilter<SecurityRequirementsOperationFilter>();
    }

    public static void UseIdentityServer(this SwaggerUIOptions options, string clientId, params string[] scopes)
    {
        UseOAuth2(options, clientId, scopes);
    }

    public static void UseOAuth2(this SwaggerUIOptions options, string clientId, params string[] scopes)
    {
        Scopes = scopes.Length > 0 ? scopes : null;

        options.OAuthClientId(clientId);

        if (Scopes is not null)
        {
            options.OAuthScopes(Scopes);
        }

        options.OAuthUsePkce();
    }
}

internal class SecurityRequirementsOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var actionMetadata = context.ApiDescription.ActionDescriptor.EndpointMetadata;
        var isAuthorized = actionMetadata.Any(item => item is AuthorizeAttribute);
        var allowAnonymous = actionMetadata.Any(item => item is AllowAnonymousAttribute);

        if (!isAuthorized || allowAnonymous)
        {
            return;
        }

        operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
        operation.Responses.Add("403", new OpenApiResponse { Description = "Forbidden" });
    }
}
