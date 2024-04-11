using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using P3AddNewFunctionalityDotNetCore.Data;

public class CustomWebApplicationFactory<TStartup>
    : WebApplicationFactory<TStartup> where TStartup : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Trouver le descripteur original du DbContext
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<P3Referential>));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            // Ajouter le DbContext avec une configuration pour une base de données en mémoire
            services.AddDbContext<P3Referential>(options =>
            {
                options.UseInMemoryDatabase("InMemoryDbForTesting");
            });

            // Assurer que la base de données est bien créée
            var sp = services.BuildServiceProvider();
            using (var scope = sp.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<P3Referential>();
                db.Database.EnsureCreated();
            }
        });
    }
}
