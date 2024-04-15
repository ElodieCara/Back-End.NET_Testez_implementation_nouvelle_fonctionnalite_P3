using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Xunit;
using Moq;
using Microsoft.Extensions.Localization;
using P3AddNewFunctionalityDotNetCore.Models.Entities;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using P3AddNewFunctionalityDotNetCore.Data;
using P3AddNewFunctionalityDotNetCore.Models;

namespace P3AddNewFunctionalityDotNetCore.Integration.Tests
{
    // Fixture de base de données pour les tests, s'assurant que chaque test a sa propre instance de base de données isolée
    public class DatabaseFixture : IDisposable
    {
        public P3Referential DbContext { get; private set; }

        public DatabaseFixture()
        {
            // Crée un nom de base de données unique pour éviter les conflits entre les tests
            var dbName = Guid.NewGuid().ToString(); 
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    // Configuration des chaînes de connexion pour la base de données
                    {"Logging:LogLevel:Default", "Warning"},
                    {"ConnectionStrings:P3Referential", "Server=.;Database=P3Referential-2f561d3b-493f-46fd-83c9-6e2643e7bd0a;Trusted_Connection=True;MultipleActiveResultSets=true"},
                    {"ConnectionStrings:P3Identity", "Server=.;Database=Identity;Trusted_Connection=True;MultipleActiveResultSets=true"}
            })
            .Build();

            var connectionString = configuration.GetConnectionString("P3Referential") ?? throw new InvalidOperationException("Connection string for 'P3Referential' is not found.");
            var options = new DbContextOptionsBuilder<P3Referential>()
                .UseSqlServer(connectionString)
                .Options;

            DbContext = new P3Referential(options,configuration);
            DbContext.Database.EnsureCreated();
            SeedDatabase(DbContext);
        }

        // Peuple la base de données avec des données initiales pour les tests
        private void SeedDatabase(P3Referential context)
        {
            context.Product.AddRange(
                new Product { 
                    Name = "Existing Product 1", 
                    Price = 20.00, 
                    Quantity = 10 },

                new Product { 
                    Name = "Existing Product 2", 
                    Price = 25.00, 
                    Quantity = 15 }
            );
            context.SaveChanges();
        }

        public void Dispose()
        {
            DbContext.Database.EnsureDeleted();
            DbContext.Dispose();
        }
    }

    // Classe de test pour ProductService utilisant DatabaseFixture pour s'assurer d'un environnement propre
    public class ProductServiceIntegrationTests : IClassFixture<DatabaseFixture>
    {
        private readonly ProductService _productService;
        private readonly P3Referential _dbContext;

        public ProductServiceIntegrationTests(DatabaseFixture fixture)
        {
            _dbContext = fixture.DbContext;
            var mockCart = new Mock<ICart>();
            var productRepository = new ProductRepository(_dbContext);
            var mockOrderRepository = new Mock<IOrderRepository>();
            var mockLocalizer = new Mock<IStringLocalizer<ProductService>>();

            // Instanciation du ProductService avec des dépendances contrôlées pour les tests
            _productService = new ProductService(mockCart.Object, productRepository, mockOrderRepository.Object, mockLocalizer.Object);
        }

        // Test pour vérifier que le produit est correctement ajouté à la base de données
        [Fact]
        public void SaveProduct_ShouldAddProductCorrectly()
        {
            // Arrange
            // Création et configuration du ViewModel de produit
            var newProductViewModel = new ProductViewModel
            {
                Name = "New Product",
                Price = "33.50",
                Stock = "25"
            };

            // Act
            // Action: Sauvegarde du nouveau produit via le service
            _productService.SaveProduct(newProductViewModel);

            // Assert
            // Récupération du produit pour vérification
            var addedProduct = _dbContext.Product.FirstOrDefault(p => p.Name == "New Product");
            Assert.NotNull(addedProduct);// Vérifie que le produit est bien ajouté
            Assert.Equal(33.50, Convert.ToDouble(addedProduct.Price), 2);// Confirme que le prix est correct avec une tolérance de 2 décimales
            Assert.Equal(25, addedProduct.Quantity);// Confirme que la quantité est correcte
        }

        // Test pour vérifier que le produit est correctement supprimé de la base de données
        [Fact]
        public void DeleteProduct_ShouldRemoveProductCorrectly()
        {
            // Arrange
            // Préparation : obtention de l'ID du premier produit
            var productId = _dbContext.Product.First().Id;

            // Act
            // Action: Suppression du produit via le service
            _productService.DeleteProduct(productId);

            // Assert
            // Vérification que le produit a bien été supprimé
            var product = _dbContext.Product.Find(productId);
            Assert.Null(product);// Le produit ne doit plus être présent dans la base
        }
    }
}
