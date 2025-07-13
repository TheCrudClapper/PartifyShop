using CSOS.Core.Domain.RepositoryContracts;
using CSOS.Core.ServiceContracts;
using Moq;
using AutoFixture;
using ComputerServiceOnlineShop.Entities.Models;
using CSOS.Core.DTO;
using CSOS.Core.ErrorHandling;
using CSOS.Core.Services;
using FluentAssertions;

namespace CSOS.Tests
{
    public class ProductImageServiceTests
    {
        private readonly IProductImageService _productImageService;
        private readonly IProductImageRepository _productImageRepository;
        private readonly Mock<IProductImageRepository> _productImageRepositoryMock;
        private readonly Fixture _fixture;

        public ProductImageServiceTests()
        {
            _productImageRepositoryMock = new Mock<IProductImageRepository>();
            _productImageRepository = _productImageRepositoryMock.Object;
            _productImageService = new ProductImageService(_productImageRepository);
            _fixture = new Fixture();
        }

        #region GetOfferPictures Method Tests
        [Fact]
        public async Task GetOfferPictures_InvalidOfferId_ReturnsEmptyList()
        {
            //Arrange
            int invalidOfferId = -1;
            List<ProductImage> productImages = [];
            _productImageRepositoryMock.Setup(item => item.GetImagesFromOfferAsync(invalidOfferId))
                .ReturnsAsync(productImages);
            
            //Act
            List<SelectListItemDto> productImagesFromService = (await _productImageService.GetOfferPictures(invalidOfferId)).ToList();
            
          
            productImagesFromService.Should().BeEmpty();
            productImagesFromService.Should().AllBeOfType<List<SelectListItemDto>>();
        }

        [Fact]
        public async Task GetOfferPictures_ValidOfferId_ReturnsProductImages()
        {
            //Arrange
            int offerId = _fixture.Create<int>();
            List<ProductImage> productImages = _fixture.Build<ProductImage>()
                .Without(item => item.Product)
                .CreateMany()
                .ToList();

            _productImageRepositoryMock.Setup(item => item.GetImagesFromOfferAsync(offerId)).ReturnsAsync(productImages);
            
            //Act
            List<SelectListItemDto> productImagesFromService = (await _productImageService.GetOfferPictures(offerId)).ToList();
            
            //Assert
            productImagesFromService.Should().NotBeEmpty();
            productImagesFromService.Should().HaveCount(productImages.Count);
            productImagesFromService.Should().AllBeOfType<SelectListItemDto>();
        }
        
        #endregion

        #region  DeleteImagesFromOffer Method Tests

        [Fact]
        public async Task DeleteImagesFromOffer_InvalidOfferId_ReturnsFailureResult()
        {
            //Arrange
            int invalidOfferId = -1;
            List<string> imageUrls = [];

            _productImageRepositoryMock.Setup(item => item.GetImagesFromOfferAsync(invalidOfferId)).ReturnsAsync([]);

            //Act
            var result = await _productImageService.DeleteImagesFromOffer(invalidOfferId, imageUrls);
            
            //Assert
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(ProductImageErrors.ProductImagesAreEmpty);
        }

        [Fact]
        public async Task DeleteImagesFromOffer_ValidOfferId_ReturnsSuccessResult()
        {
            //Arrange
            int validOfferId = _fixture.Create<int>();
            
            //List holds image Urls to delete (deactivate)
            List<string> imageUrls = new List<string>()
            {
                new string("test123/test123/1.png"),
                new string("tescik/12/2.png"),
            };
            
            //List holds testable images
            List<ProductImage> productImages = new List<ProductImage>()
            {
                _fixture.Build<ProductImage>()
                    .With(item => item.ImagePath, imageUrls[0] )
                    .With(item => item.IsActive, true)
                    .Without(item => item.DateDeleted)
                    .Without(item => item.Product)
                    .Create(),
                
                _fixture.Build<ProductImage>()
                    .With(item => item.ImagePath, imageUrls[1] )
                    .With(item => item.IsActive, true)
                    .Without(item => item.DateDeleted)
                    .Without(item => item.Product)
                    .Create(),
            };

            _productImageRepositoryMock.Setup(item => item.GetImagesFromOfferAsync(validOfferId))
                .ReturnsAsync(productImages);
                
            
            //Act
            var result = await _productImageService.DeleteImagesFromOffer(validOfferId, imageUrls);
            
            //Assert
            result.IsSuccess.Should().BeTrue();
            productImages.ForEach(item => item.IsActive.Should().BeFalse());
            productImages.ForEach(item => item.DateDeleted.Should().NotBeNull());
        }
        #endregion
        
    }
}

