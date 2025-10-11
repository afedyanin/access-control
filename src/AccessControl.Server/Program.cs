using System.Text.Json.Serialization;
using AccessControl.DataAccess;
using AccessControl.WebApi;
using AccessControl.WebApi.Authorization;
using Microsoft.OpenApi.Models;

namespace AccessControl.Server;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers()
            .AddJsonOptions(options =>
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()))
            .AddApplicationPart(typeof(FeatureKeysController).Assembly);

        var configuration = builder.Configuration;
        var connectionString = configuration.GetConnectionString("AccessControlDbConnection");

        builder.Services.AddAccessCotrolDataAccess(connectionString!);
        builder.Services.AddApiKeyuthorization();

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "My API V1", Version = "v1" });
            options.SwaggerDoc(ApiKeyConsts.ApiGroupName, new OpenApiInfo { Title = "Access Control Admin API", Version = "1.0" });

            options.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
            {
                Description = "API Key authorization header, e.g. \"X-API-Key: YOUR_API_KEY\"",
                Name = ApiKeyConsts.ApiKeyHeaderName,
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "ApiKeyScheme"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "ApiKey"
                        },
                        Scheme = "oauth2",
                        Name = "ApiKey",
                        In = ParameterLocation.Header,
                    },
                    new List<string>()
                }
            });
        });

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.SwaggerEndpoint($"/swagger/{ApiKeyConsts.ApiGroupName}/swagger.json", "Access Control Admin API");
            });
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
