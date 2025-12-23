using AccessControl.AdminUI.Components;
using AccessControl.Contracts;
using Microsoft.FluentUI.AspNetCore.Components;
using Refit;

namespace AccessControl.AdminUI;

public static class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorComponents()
            .AddInteractiveServerComponents();
        builder.Services.AddFluentUIComponents();

        builder.Services.AddRefitClient<IAccessControlClient>()
               .ConfigureHttpClient(c =>
               {
                   c.BaseAddress = new Uri(ApiConsts.BaseUrl);
                   c.DefaultRequestHeaders.Add(ApiConsts.ApiKeyHeaderName, ApiConsts.ApiKey);
               });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error", createScopeForErrors: true);
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseStaticFiles();
        app.UseAntiforgery();

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();

        app.Run();
    }
}

internal static class ApiConsts
{
    public const string BaseUrl = "https://localhost:7082";

    public const string ApiKeyHeaderName = "X-API-Key";
    public const string ApiKey = "6CBxzdYcEgNDrRh945DpkBF7e4d4Kib34dwL9ZE5egiL0iL5Y3dzREUBSU56UwUkN";
}