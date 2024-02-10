using Microsoft.Extensions.DependencyInjection;
using ServerConfigurator;
using ServerConfigurator.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register services for Dependency Injection
// read the config file path from the appsettings.json
var configFilePath = builder.Configuration["ConfigFilePath"];
// create a new instance of the ConfigFileReaderWriter with the config file path
builder.Services.AddScoped<IConfigFileReaderWriter, ConfigFileReaderWriter>(p => new ConfigFileReaderWriter(configFilePath));

builder.Services.AddScoped<IConfigService, ConfigService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
