using System;
using System.Linq;
using System.Web;
using Ctyar.Swashbuckle.Auth;
using Microsoft.AspNetCore.Builder;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Microsoft.Extensions.DependencyInjection;

public static class SwaggerOptionsExtensions
{
    private static string[]? Scopes;

    /// <summary>
    /// Add Duende Identity Server OAuth 2 security definition and a global security requirement, to the generated Swagger
    /// </summary>
    /// <param name="swaggerGenOptions"></param>
    /// <param name="authority">The authority URL to be used for authorizationCode OAuth flow.</param>
    public static void AddIdentityServer(this SwaggerGenOptions swaggerGenOptions, string authority)
    {
        var authoritUrl = new Uri(authority);

        AddOAuth2(swaggerGenOptions, new Uri(authoritUrl, "connect/authorize"), new Uri(authoritUrl, "connect/token"));
    }

    /// <summary>
    /// Add Auth0 OAuth 2 security definition and a global security requirement, to the generated Swagger
    /// </summary>
    /// <param name="swaggerGenOptions"></param>
    /// <param name="authority">The authority URL to be used for authorizationCode OAuth flow.</param>
    /// <param name="audience">The audience to be used for authorizationCode OAuth flow.</param>
    public static void AddAuth0(this SwaggerGenOptions swaggerGenOptions, string authority, string audience)
    {
        var authorityUrl = new Uri(authority);

        var httpValueCollection = HttpUtility.ParseQueryString(authorityUrl.Query);
        httpValueCollection.Add("audience", audience);

        var authorizationUrl = new UriBuilder(authorityUrl)
        {
            Query = httpValueCollection.ToString(),
            Path = "authorize"
        }.Uri;

        var tokenUrl = new UriBuilder(authorityUrl)
        {
            Path = "oauth/token"
        }.Uri;

        AddOAuth2(swaggerGenOptions, authorizationUrl, tokenUrl);
    }

    /// <summary>
    /// Add OAuth 2 security definitions and a global security requirement, to the generated Swagger
    /// </summary>
    /// <param name="swaggerGenOptions"></param>
    /// <param name="authorizationUrl">The authorization URL to be used for authorizationCode OAuth flow.</param>
    /// <param name="tokenUrl">The token URL to be used for authorizationCode OAuth flow.</param>
    public static void AddOAuth2(this SwaggerGenOptions swaggerGenOptions, Uri authorizationUrl, Uri tokenUrl)
    {
        swaggerGenOptions.AddSecurityDefinition("oAuth2", new OpenApiSecurityScheme
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

        swaggerGenOptions.AddSecurityRequirement(new OpenApiSecurityRequirement
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

        swaggerGenOptions.OperationFilter<SecurityRequirementsOperationFilter>();
    }

    /// <summary>
    /// Set the Duende Identity Server clientId and scopes for the authorizatonCode flow with proof Key for Code Exchange.
    /// </summary>
    /// <param name="options"></param>
    /// <param name="clientId">Default clientId</param>
    /// <param name="scopes">String array of initially selected OAuth scopes, default is empty array</param>
    public static void UseIdentityServer(this SwaggerUIOptions options, string clientId, params string[] scopes)
    {
        UseOAuth2(options, clientId, scopes);
    }

    /// <summary>
    /// Set the Auth0 clientId and scopes for the authorizatonCode flow with proof Key for Code Exchange.
    /// </summary>
    /// <param name="options"></param>
    /// <param name="clientId">Default clientId</param>
    /// <param name="scopes">String array of initially selected OAuth scopes, default is empty array</param>
    public static void UseAuth0(this SwaggerUIOptions options, string clientId, params string[] scopes)
    {
        UseOAuth2(options, clientId, scopes);
    }

    /// <summary>
    /// Set the clientId and scopes for the authorizatonCode flow with proof Key for Code Exchange.
    /// </summary>
    /// <param name="options"></param>
    /// <param name="clientId">Default clientId</param>
    /// <param name="scopes">String array of initially selected OAuth scopes, default is empty array</param>
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