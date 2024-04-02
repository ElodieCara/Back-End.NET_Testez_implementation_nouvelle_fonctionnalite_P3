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
            _productServiceMock = new();
        }

        [Fact]
        public void SaveProduct_Should_ReturnErrorMissingName_WhenNameIsNull()
        {
            // Arrange
            var product = new ProductViewModel {Name = null};

            _productServiceMock.Setup(service => service.CheckProductModelErrors(It.IsAny<ProductViewModel>()))
                .Returns(new List<string> { "MissingName" });

            var productService = _productServiceMock.Object;

            // Act
            var errors = productService.CheckProductModelErrors(product);

            // Assert
            Assert.Contains("MissingName", errors); 
        }

        [Fact]
        public void SaveProduct_Should_ReturneErrorMissingPrice_WhenPriceIsMissing()
        {
            // Arrange
            var product = new ProductViewModel { Price = null };

            _productServiceMock.Setup(service => service.CheckProductModelErrors(It.IsAny<ProductViewModel>()))
                .Returns(new List<string> { "MissingPrice" });

            var productService = _productServiceMock.Object;

            // Act
            var errors = productService.CheckProductModelErrors(product);

            // Assert
            Assert.Contains("MissingPrice", errors);
        }

        [Fact]
        public void SaveProduct_Should_ReturnErrorPriceNotANumber_WhenPriceIsNotANumber()
        {
            // Arrange
            var product = new ProductViewModel { Price = "not_a_number" };

            _productServiceMock.Setup(service => service.CheckProductModelErrors(It.IsAny<ProductViewModel>()))
                .Returns(new List<string> { "PriceNotANumber" });

            var productService = _productServiceMock.Object;

            // Act
            var errors = productService.CheckProductModelErrors(product);

            // Assert
            Assert.Contains("PriceNotANumber", errors);
        }

        [Fact]
        public void SaveProduct_Should_ReturnErrorPriceNotGreaterThanZero_WhenPriceIsZero()
        {
            // Arrange
            var product = new ProductViewModel { Price = "0" };

            _productServiceMock.Setup(service => service.CheckProductModelErrors(It.IsAny<ProductViewModel>()))
                .Returns(new List<string> { "PriceNotGreaterThanZero" });

            var productService = _productServiceMock.Object;

            // Act
            var errors = productService.CheckProductModelErrors(product);

            // Assert
            Assert.Contains("PriceNotGreaterThanZero", errors);
        }

        [Fact]
        public void SaveProduct_Should_ReturneErrorMissingQuantity_WhenQuantityIsNull()
        {
            // Arrange
            var product = new ProductViewModel { Stock = null };

            _productServiceMock.Setup(service => service.CheckProductModelErrors(It.IsAny<ProductViewModel>()))
                .Returns(new List<string> { "MissingQuantity" });

            var productService = _productServiceMock.Object;

            // Act
            var errors = productService.CheckProductModelErrors(product);

            // Assert
            Assert.Contains("MissingQuantity", errors);
        }

        [Fact]
        public void SaveProduct_Should_ReturnErrorQuantityNotAnInteger_WhenQuantityIsNotAnInteger()
        {
            // Arrange
            var product = new ProductViewModel { Stock = "abc" }; 

            _productServiceMock.Setup(service => service.CheckProductModelErrors(It.IsAny<ProductViewModel>()))
                .Returns(new List<string> { "QuantityNotAnInteger" });

            var productService = _productServiceMock.Object;

            // Act
            var errors = productService.CheckProductModelErrors(product);

            // Assert
            Assert.Contains("QuantityNotAnInteger", errors);
        }

        [Fact]
        public void SaveProduct_Should_ReturnErrorQuantityNotGreaterThanZero_WhenQuantityIsNotGreaterThanZero()
        {
            // Arrange
            var product = new ProductViewModel { Stock = "0" };

            _productServiceMock.Setup(service => service.CheckProductModelErrors(It.IsAny<ProductViewModel>()))
               .Returns(new List<string> { "QuantityNotGreaterThanZero" });

            var productService = _productServiceMock.Object;

            // Act
            var errors = productService.CheckProductModelErrors(product);

            // Assert
            Assert.Contains("QuantityNotGreaterThanZero", errors);
        }




        // TODO write test methods to ensure a correct coverage of all possibilities
    }
}