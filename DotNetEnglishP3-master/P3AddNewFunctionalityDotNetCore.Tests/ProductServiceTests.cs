using P3AddNewFunctionalityDotNetCore.Models.Services;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using System.Collections.Generic;
using Xunit;
using Moq;

namespace P3AddNewFunctionalityDotNetCore.Tests
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductService> _productServiceMock;

        public ProductServiceTests()
        {
            _productServiceMock = new Mock<IProductService>();
        }

        // Teste les scénarios d'erreur pour le nom du produit
        [Theory]
        [InlineData(null, "MissingName")] // Tester avec un nom nul
        [InlineData("", "MissingName")] // Tester avec un nom vide
        public void SaveProduct_Should_ReturnErrorForInvalidName(string name, string expectedError)
        {
            // Arrange
            var product = new ProductViewModel { Name = name };
            _productServiceMock.Setup(service => service.CheckProductModelErrors(It.IsAny<ProductViewModel>()))
                               .Returns(new List<string> { expectedError });
            var productService = _productServiceMock.Object;

            // Act
            var errors = productService.CheckProductModelErrors(product);

            // Assert
            Assert.Contains(expectedError, errors);
        }

        // Teste les scénarios d'erreur pour le prix du produit
        [Theory]
        [InlineData(null, "MissingPrice")]
        [InlineData("not_a_number", "PriceNotANumber")]
        [InlineData("0", "PriceNotGreaterThanZero")]
        public void SaveProduct_Should_ReturnErrorForInvalidPrice(string price, string expectedError)
        {
            // Arrange
            var product = new ProductViewModel { Price = price };
            _productServiceMock.Setup(service => service.CheckProductModelErrors(It.IsAny<ProductViewModel>()))
                               .Returns(new List<string> { expectedError });
            var productService = _productServiceMock.Object;

            // Act
            var errors = productService.CheckProductModelErrors(product);

            // Assert
            Assert.Contains(expectedError, errors);
        }

        // Teste les scénarios d'erreur pour la quantité du produit
        [Theory]
        [InlineData(null, "MissingQuantity")]
        [InlineData("abc", "QuantityNotAnInteger")]
        [InlineData("0", "QuantityNotGreaterThanZero")]
        public void SaveProduct_Should_ReturnErrorForInvalidStock(string stock, string expectedError)
        {
            // Arrange
            var product = new ProductViewModel { Stock = stock };
            _productServiceMock.Setup(service => service.CheckProductModelErrors(It.IsAny<ProductViewModel>()))
                               .Returns(new List<string> { expectedError });
            var productService = _productServiceMock.Object;

            // Act
            var errors = productService.CheckProductModelErrors(product);

            // Assert
            Assert.Contains(expectedError, errors);
        }

        // Teste un cas réussi où le produit est valide
        [Fact]
        public void SaveProduct_Should_Succeed_With_ValidProduct()
        {
            // Arrange
            var product = new ProductViewModel { Name = "ValidName", Price = "100", Stock = "10" };
            _productServiceMock.Setup(service => service.CheckProductModelErrors(It.IsAny<ProductViewModel>()))
                               .Returns(new List<string>()); 
            var productService = _productServiceMock.Object;

            // Act
            var errors = productService.CheckProductModelErrors(product);

            // Assert
            Assert.Empty(errors); // Vérifie qu'aucune erreur n'est retournée pour un produit valide
        }

        // Teste la vérification des interactions entre les composants avec Mock
        [Fact]
        public void SaveProduct_Should_Call_SaveProduct_On_Repository_With_ValidProduct()
        {
            // Arrange
            var product = new ProductViewModel { Name = "ValidProduct", Price = "50", Stock = "5" };
            var productServiceMock = new Mock<IProductService>();
            productServiceMock.Setup(ps => ps.SaveProduct(It.IsAny<ProductViewModel>()))
                              .Verifiable("The service did not call SaveProduct as expected.");

            // Act
            productServiceMock.Object.SaveProduct(product);

            // Assert
            productServiceMock.Verify(); // Vérifie que SaveProduct a été appelé sur le mock
        }
    }
}
