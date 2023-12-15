# Swashbuckle Auth

[![Build Status](https://ctyar.visualstudio.com/Swashbuckle.Auth/_apis/build/status%2Fctyar.Swashbuckle.Auth?branchName=main)](https://ctyar.visualstudio.com/Swashbuckle.Auth/_build/latest?definitionId=9&branchName=main)
[![Ctyar.Swashbuckle.Auth](https://img.shields.io/nuget/v/Ctyar.Swashbuckle.Auth.svg)](https://www.nuget.org/packages/Ctyar.Swashbuckle.Auth/)

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

## Demo
![Demo](https://user-images.githubusercontent.com/1432648/227712843-8ffd08f7-0489-46f6-9e46-aeec6e99dac4.gif)

## Build
[Install](https://get.dot.net) the [required](global.json) .NET SDK.

Run:
```
$ dotnet build
```
