using Moq;
using P3AddNewFunctionalityDotNetCore.Models.Entities;
using P3AddNewFunctionalityDotNetCore.Models.Repositories;
using P3AddNewFunctionalityDotNetCore.Models.Services;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;

namespace P3AddNewFunctionalityDotNetCore.Integration.Tests
{
    public class ProductServiceIntegrationTest
    {
        [Fact]
        public void ProductChangesByAdmin_ShouldReflectOnClientSide()
        {
           
            // Arrange - Configuration initiale avec les repositories mockés et les services
            var mockProductRepository = new Mock<IProductRepository>();
            // Simuler des produits existants pour la configuration du test
            var existingProducts = new List<Product>
            {
                new Product { Id = 1, Name = "Existing Product 1", Price = 20.00, Quantity = 10 },
                new Product { Id = 2, Name = "Existing Product 2", Price = 25.00, Quantity = 15 }
            };

            mockProductRepository.Setup(repo => repo.GetAllProducts()).Returns(existingProducts);

            // Simuler l'ajout d'un nouveau produit
            mockProductRepository.Setup(repo => repo.SaveProduct(It.IsAny<Product>()))
                                 .Callback<Product>((product) =>
                                 {
                                     product.Id = 3; // Assigner un nouvel ID pour simplifier
                                     existingProducts.Add(product);
                                 });

            // Simuler la suppression d'un produit
            mockProductRepository.Setup(repo => repo.DeleteProduct(It.IsAny<int>()))
                                 .Callback<int>((id) =>
                                 {
                                     var productToRemove = existingProducts.FirstOrDefault(p => p.Id == id);
                                     if (productToRemove != null) existingProducts.Remove(productToRemove);
                                 });

            var productService = new ProductService(null, mockProductRepository.Object, null, null);

            // Act - L'admin ajoute un nouveau produit
            var newProductViewModel = new ProductViewModel
            {
                Name = "New Test Product",
                Description = "Test Description",
                Details = "Test Details",
                Price = "30.99",
                Stock = "20"
            };

            productService.SaveProduct(newProductViewModel);

            // L'admin supprime un produit existant
            productService.DeleteProduct(1); // Supposons que cet ID de produit existe pour ce test

            // Récupérer la liste des produits mise à jour pour refléter la vue client
            var updatedProducts = productService.GetAllProducts();

            // Assert
            // Vérifier que le nouveau produit est ajouté
            Assert.Contains(updatedProducts, p => p.Name == "New Test Product");
            // Vérifier que le produit supprimé n'est plus présent
            Assert.DoesNotContain(updatedProducts, p => p.Id == 1);
            // Vérifier qu'il n'y a pas d'incohérences - le nombre de produits devrait maintenant être le compte original + 1 - 1
            Assert.Equal(existingProducts.Count, updatedProducts.Count);
        }
    }
}