using System.Text.Json.Serialization;
using AccessControl.WebApi;
using AccessControl.DataAccess;

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

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
