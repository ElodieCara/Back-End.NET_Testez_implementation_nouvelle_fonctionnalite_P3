using P3AddNewFunctionalityDotNetCore.Models.Services;
using P3AddNewFunctionalityDotNetCore.Models.ViewModels;
using Xunit;

namespace P3AddNewFunctionalityDotNetCore.Tests
{
    public class ProductServiceTests
    {
        private readonly ProductService _productService;
        /// <summary>
        /// Take this test method as a template to write your test method.
        /// A test method must check if a definite method does its job:
        /// returns an expected value from a particular set of parameters
        /// </summary>
        [Fact]
        public void SaveProduct_Should_ReturnErrorMissingName()
        {
            // Arrange
            var product = new ProductViewModel(); 

            // Act
            var errors = _productService.CheckProductModelErrors(product);

            // Assert
            Assert.Contains("MissingName", errors); 
        }

        [Fact]
        public void SaveProduct_Should_ReturneError_MissingPrice()
        {
            // Arrange
            var product = new ProductViewModel(); 

            // Act
            var errors = _productService.CheckProductModelErrors(product);

            // Assert
            Assert.Contains("MissingPrice", errors);
        }

        [Fact]
        public void SaveProduct_Should_ReturneError_PriceNotANumber()
        {
            // Arrange
            var product = new ProductViewModel(); 

            // Act
            var errors = _productService.CheckProductModelErrors(product);

            // Assert
            Assert.Contains("PriceNotANumber", errors);
        }

        [Fact]
        public void SaveProduct_Should_ReturneError_PriceNotGreaterThanZero()
        {
            // Arrange
            var product = new ProductViewModel(); 

            // Act
            var errors = _productService.CheckProductModelErrors(product);

            // Assert
            Assert.Contains("PriceNotGreaterThanZero", errors);
        }
        [Fact]
        public void SaveProduct_Should_ReturneError_MissingQuantity()
        {
            // Arrange
            var product = new ProductViewModel();

            // Act
            var errors = _productService.CheckProductModelErrors(product);

            // Assert
            Assert.Contains("MissingQuantity", errors);
        }

        [Fact]
        public void SaveProduct_Should_ReturneError_QuantityNotAnInteger()
        {
            // Arrange
            var product = new ProductViewModel();

            // Act
            var errors = _productService.CheckProductModelErrors(product);

            // Assert
            Assert.Contains("QuantityNotAnInteger", errors);
        }

        [Fact]
        public void SaveProduct_Should_ReturneError_QuantityNotGreaterThanZero()
        {
            /// Arrange
            var product = new ProductViewModel();

            // Act
            var errors = _productService.CheckProductModelErrors(product);

            // Assert
            Assert.Contains("QuantityNotGreaterThanZero", errors);
        }



        // TODO write test methods to ensure a correct coverage of all possibilities
    }
}