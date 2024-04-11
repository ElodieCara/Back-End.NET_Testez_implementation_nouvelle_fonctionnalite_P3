using System.Threading.Tasks;
using Xunit;
using Microsoft.EntityFrameworkCore;
using P3AddNewFunctionalityDotNetCore.Data;
using P3AddNewFunctionalityDotNetCore.Models.Entities;
using Microsoft.Extensions.DependencyInjection;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;

public class ProductServiceCrudIntegrationTest : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly CustomWebApplicationFactory<Program> _factory;

    public ProductServiceCrudIntegrationTest(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task CreateProduct_ShouldAddProductCorrectly()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<P3Referential>();
        dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();

        var productService = scope.ServiceProvider.GetRequiredService<IProductService>();
        var newProductViewModel = new ProductViewModel
        {
            Name = "New Product",
            Description = "A new product",
            Details = "Details of the new product",
            Price = "29.99",
            Stock = "100"
        };

        // Act
        productService.SaveProduct(newProductViewModel);
        await dbContext.SaveChangesAsync();

        // Assert
        var product = await dbContext.Product.FirstOrDefaultAsync(p => p.Name == "New Product");
        Assert.NotNull(product);
        Assert.Equal("A new product", product.Description);
    }

    [Fact]
    public async Task UpdateProductStocks_ShouldUpdateProductQuantityCorrectly()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<P3Referential>();
        dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();

        // Ajoutez des produits avec des quantités initiales
        SeedData.Initialize(scope.ServiceProvider, null);

        // Act
        var productService = scope.ServiceProvider.GetRequiredService<IProductService>();
        var productRepository = scope.ServiceProvider.GetRequiredService<IProductRepository>();

        // Modifiez la quantité du produit
        productRepository.UpdateProductStocks(1, 5); // Retirez 5 de la quantité initiale
        await dbContext.SaveChangesAsync(); // Sauvegardez les changements dans la base de données

        // Assert
        // Vérifiez si la quantité mise à jour est correcte côté client
        var updatedProduct = await productService.GetProduct(1); // Récupérez le produit mis à jour
        Assert.Equal(5, updatedProduct.Quantity); // La quantité mise à jour doit être 5
    }

    [Fact]
    public async Task DeleteProduct_ShouldRemoveProductCorrectly()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<P3Referential>();
        dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();

        var product = new Product { Name = "Product To Delete", Price = 30.00, Quantity = 20 };
        dbContext.Product.Add(product);
        await dbContext.SaveChangesAsync();

        // Act
        dbContext.Product.Remove(product);
        await dbContext.SaveChangesAsync();

        // Assert
        var deletedProduct = await dbContext.Product.FindAsync(product.Id);
        Assert.Null(deletedProduct);
    }

    [Fact]
    public async Task GetProducts_ShouldReturnAllProducts()
    {
        // Arrange
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<P3Referential>();
        dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();

        dbContext.Product.AddRange(new Product[]
        {
            new Product { Name = "Product 1", Price = 10.00, Quantity = 100 },
            new Product { Name = "Product 2", Price = 20.00, Quantity = 200 }
        });
        await dbContext.SaveChangesAsync();

        // Act
        var products = await dbContext.Product.ToListAsync();

        // Assert
        Assert.Equal(2, products.Count);
        Assert.Contains(products, p => p.Name == "Product 1");
        Assert.Contains(products, p => p.Name == "Product 2");
    }
}
