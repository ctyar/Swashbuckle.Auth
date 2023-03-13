# Swashbuckle Auth

[![Build Status](https://ctyar.visualstudio.com/Swashbuckle/_apis/build/status/ctyar.Swashbuckle?branchName=main)](https://ctyar.visualstudio.com/Swashbuckle/_build/latest?definitionId=6&branchName=main)

A package to simplify adding authentication to the Swagger.

## Usage

**Auth0**
```csharp
builder.Services.AddSwaggerGen(c =>
{
    c.AddAuth0(authority, audience);
});

app.UseSwaggerUI(c =>
{
    c.UseAuth0(clientId, scopes);
});
```

**Identity Server**
```csharp
builder.Services.AddSwaggerGen(c =>
{
    c.AddIdentityServer(authority);
});

app.UseSwaggerUI(c =>
{
    c.UseIdentityServer(clientId, scopes);
});
```

**OAuth2**
```csharp
builder.Services.AddSwaggerGen(c =>
{
    c.AddOAuth2(authorizationUrl, tokenUrl);
});

app.UseSwaggerUI(c =>
{
    c.UseOAuth2(clientId, scopes);
});
```

## Build
[Install](https://get.dot.net) the [required](global.json) .NET SDK.

Run:
```
$ dotnet build
```
