using API.DbContexts;
using API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Net.Http.Headers;
using System.Net.Mime;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var requireAuthenticatedUserPolicy = new AuthorizationPolicyBuilder()
             .RequireAuthenticatedUser()
             .Build();

builder.Services.AddControllers(options =>
{
    options.Filters.Add(new AuthorizeFilter(requireAuthenticatedUserPolicy));
});
builder.Services.AddAuthentication().AddJwtBearer();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<WeatherForecastDbContext>();
builder.Services.AddTransient<IExternalAPIService, ExternalAPIService>();
builder.Services.AddHttpClient("WeatherClient", client =>
{
    client.BaseAddress = new Uri(builder.Configuration.GetValue<string>("WeatherAPIUrl")!);
    client.DefaultRequestHeaders.Clear();
    client.DefaultRequestHeaders.Add(HeaderNames.Accept, MediaTypeNames.Application.Json);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<WeatherForecastDbContext>();
    context.Database.EnsureCreated();
}

app.Run();

public partial class Program { }
