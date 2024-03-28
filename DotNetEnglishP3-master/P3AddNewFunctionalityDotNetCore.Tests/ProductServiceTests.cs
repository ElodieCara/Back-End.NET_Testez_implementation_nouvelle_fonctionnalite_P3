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
        public void SaveProduct_Should_ReturneError_MissingPrice()
        {
            // Arrange
            var product = new ProductViewModel(); 

            // Act
            var errors = productService.CheckProductModelErrors(product);

            // Assert
            Assert.Contains("MissingPrice", errors);
        }

        [Fact]
        public void SaveProduct_Should_ReturneError_PriceNotANumber()
        {
            // Arrange
            var product = new ProductViewModel(); 

            // Act
            var errors = productService.CheckProductModelErrors(product);

            // Assert
            Assert.Contains("PriceNotANumber", errors);
        }

        [Fact]
        public void SaveProduct_Should_ReturneError_PriceNotGreaterThanZero()
        {
            // Arrange
            var product = new ProductViewModel(); 

            // Act
            var errors = productService.CheckProductModelErrors(product);

            // Assert
            Assert.Contains("PriceNotGreaterThanZero", errors);
        }

        [Fact]
        public void SaveProduct_Should_ReturneError_MissingQuantity()
        {
            // Arrange
            var product = new ProductViewModel();

            // Act
            var errors = productService.CheckProductModelErrors(product);

            // Assert
            Assert.Contains("MissingQuantity", errors);
        }

        [Fact]
        public void SaveProduct_Should_ReturneError_QuantityNotAnInteger()
        {
            // Arrange
            var product = new ProductViewModel();

            // Act
            var errors = productService.CheckProductModelErrors(product);

            // Assert
            Assert.Contains("QuantityNotAnInteger", errors);
        }

        [Fact]
        public void SaveProduct_Should_ReturneError_QuantityNotGreaterThanZero()
        {
            /// Arrange
            var product = new ProductViewModel();

            // Act
            var errors = productService.CheckProductModelErrors(product);

            // Assert
            Assert.Contains("QuantityNotGreaterThanZero", errors);
        }



        // TODO write test methods to ensure a correct coverage of all possibilities
    }
}