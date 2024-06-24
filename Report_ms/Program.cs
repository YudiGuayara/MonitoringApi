using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MonitoringApi.Models;
using MonitoringApi.Services;
using MongoDB.Driver;
using Microsoft.Extensions.Options; 

var builder = WebApplication.CreateBuilder(args);

// Configurar MongoDB
builder.Services.Configure<ReportsDatabaseSettings>(
    builder.Configuration.GetSection("ReportsDatabaseSettings"));
builder.Services.AddSingleton<IMongoClient>(s =>
{
    var settings = s.GetRequiredService<IOptions<ReportsDatabaseSettings>>().Value;
    return new MongoClient(settings.ConnectionString);
});
builder.Services.AddSingleton<ReportsService>();

builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configurar el pipeline de solicitudes HTTP.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles(); // Servir archivos estáticos desde wwwroot

app.UseAuthorization();

app.MapControllers();

// Ruta para servir tu archivo HTML estático
app.MapFallbackToFile("/index.html");

app.Run();
