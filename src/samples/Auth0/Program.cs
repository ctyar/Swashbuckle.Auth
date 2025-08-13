using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Sample;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(o =>
            {
                o.Authority = "https://dev-8y4ruiprjbfy2krb.eu.auth0.com/";
                o.Audience = "http://localhost:5148/swagger/";
            });

        builder.Services.AddAuthorization();

        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.AddAuth0("https://dev-8y4ruiprjbfy2krb.eu.auth0.com", "http://localhost:5148/swagger/");
        });

        var app = builder.Build();

        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.UseAuth0("Z52Qt91VmqlDkVTRv2OSRVzXlQmwWNzr", "openid", "profile", "email", "api", "offline_access");
        });

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}