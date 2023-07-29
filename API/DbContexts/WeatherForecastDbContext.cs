using Microsoft.EntityFrameworkCore;

namespace API.DbContexts;

public class WeatherForecastDbContext : DbContext
{
    public DbSet<WeatherForecast> WeatherForecasts { get; set; }

    public WeatherForecastDbContext(DbContextOptions<WeatherForecastDbContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseInMemoryDatabase("ILoveDotNetFunctionalTest");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WeatherForecast>()
                    .HasData(
                        new WeatherForecast
                        {
                            Id = 1,
                            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
                            TemperatureC = 1,
                            Summary = "Freezing"
                        });

        base.OnModelCreating(modelBuilder);
    }
}
