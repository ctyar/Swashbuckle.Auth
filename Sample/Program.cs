
namespace Sample;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(c =>
        {
            c.AddOAuth2(new Uri("https://demo.duendesoftware.com/connect/authorize"), new Uri("https://demo.duendesoftware.com//connect/token"));
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.UseOAuth2("interactive.public", "openid", "profile", "offline_access");
            });
        }

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
