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

        [Theory]
        [InlineData(null, "MissingName")]
        [InlineData("", "MissingName")] 
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
        
    }
}
