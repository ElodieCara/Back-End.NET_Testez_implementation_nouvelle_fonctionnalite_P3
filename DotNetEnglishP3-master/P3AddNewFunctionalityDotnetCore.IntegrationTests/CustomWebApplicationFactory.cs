using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using P3AddNewFunctionalityDotNetCore.Data;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Configuration de l'authentification fictive
            services.AddAuthentication("TestScheme")
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("TestScheme", options => { });

            // Supprimer la configuration de la base de données existante pour P3Referential
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<P3Referential>));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            // Ajouter la base de données en mémoire pour les tests
            services.AddDbContext<P3Referential>(options =>
            {
                options.UseInMemoryDatabase("InMemoryAppDb");
            });

        });
    }
}






//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Mvc.Testing;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.DependencyInjection;
//using P3AddNewFunctionalityDotNetCore.Data;

//public class CustomWebApplicationFactory : WebApplicationFactory<Program>
//{
//    protected override void ConfigureWebHost(IWebHostBuilder builder)
//    {
//        builder.ConfigureServices(services =>
//        {
//            services.AddAuthentication("TestScheme")
//                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("TestScheme", options => { });

//            // Configurer la base de données en mémoire pour les tests
//            var serviceProvider = new ServiceCollection()
//                .AddEntityFrameworkInMemoryDatabase()
//                .BuildServiceProvider();

//            services.AddDbContext<P3Referential>(options =>
//            {
//                options.UseInMemoryDatabase("InMemoryAppDb");
//                options.UseInternalServiceProvider(serviceProvider);
//            });

//        });
//    }
//}
