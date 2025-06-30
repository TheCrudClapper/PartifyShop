using AutoFixture;
using ComputerServiceOnlineShop.Entities.Models;
using ComputerServiceOnlineShop.ServiceContracts;
using ComputerServiceOnlineShop.Services;
using CSOS.Core.Domain.RepositoryContracts;
using CSOS.Core.ServiceContracts;
using FluentAssertions;
using Moq;
using CSOS.Core.ErrorHandling;
namespace CSOS.Tests
{
    public class CartServiceTests
    {
        private readonly ICartService _cartService;
        private readonly ICartRepository _cartRepository;
        private readonly IOfferRepository _offerRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ICurrentUserService> _currentUserServiceMock;
        private readonly Mock<IOfferRepository> _offerRepositoryMock;
        private readonly Mock<ICartRepository> _cartRepositoryMock;
        private readonly IFixture _fixture;
        public CartServiceTests()
        {
            //should be mocked
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _cartRepositoryMock = new Mock<ICartRepository>();
            _offerRepositoryMock = new Mock<IOfferRepository>();
            _currentUserServiceMock = new Mock<ICurrentUserService>();
            _cartRepository = _cartRepositoryMock.Object;
            _unitOfWork = _unitOfWorkMock.Object;
            _currentUserService = _currentUserServiceMock.Object;
            _offerRepository = _offerRepositoryMock.Object;
            _cartService = new CartService(_currentUserService, _offerRepository, _cartRepository, _unitOfWork);
            _fixture = new Fixture();

            _fixture.Behaviors
            .OfType<ThrowingRecursionBehavior>()
            .ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
            
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        #region DeleteFromCart Method Tests

        [Fact]
        public async void DeleteFromCart_InvalidCartId_ReturnsFailureResult()
        {
            //Arrange
            int cartItemId = _fixture.Create<int>();
            
            _cartRepositoryMock.Setup(item => item.GetCartItemByIdAsync(cartItemId))
                .ReturnsAsync((CartItem?)null);

            //Act
            Result result = await _cartService.DeleteFromCart(cartItemId);

            //Assert
            result.Should().NotBeNull();
            result.IsFailure.Should().BeTrue();
            result.Error.Should().Be(CartItemErrors.CartItemDoesNotExists);
        }

        [Fact]
        public async void DeleteFromCart_ValidCartId_ReturnsSuccessResult()
        {
            var cartItem = _fixture.Build<CartItem>()
                .With(item => item.IsActive, true)
                .Without(item => item.DateDeleted)
                .Create();
            
            
            _cartRepositoryMock.Setup(repo => repo.GetCartItemByIdAsync(cartItem.Id))
                .ReturnsAsync(cartItem);

            _cartRepositoryMock.Setup(repo => repo.GetCartByIdAsync(cartItem.CartId))
                .ReturnsAsync(_fixture.Build<Cart>().With(c => c.Id, cartItem.CartId).Create());

            _cartRepositoryMock.Setup(repo => repo.GetCartItemsForCostsUpdateAsync(cartItem.CartId))
                .ReturnsAsync(new List<CartItem> { cartItem });

            _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1); 
            
            // Act
            var result = await _cartService.DeleteFromCart(cartItem.Id);

            // Assert
            result.Should().NotBeNull();
            result.IsSuccess.Should().BeTrue();
            cartItem.IsActive.Should().BeFalse();
            cartItem.DateDeleted.Should().NotBeNull();
        }
        #endregion 
        #region CalculateItemsTotal Method Tests
        [Fact]
        public void CalculateItemsTotal_WithValidItems_ReturnsCorrectTotal()
        {
            //Arrange
            IEnumerable<CartItem> cartItems = new List<CartItem>()
            {
                
                new CartItem()
                {
                    Quantity = 15,
                    Offer = new Offer { Price = 200 }
                },
                new CartItem()
                {
                    Quantity = 1,
                    Offer = new Offer { Price = 1000 }
                },
                new CartItem()
                {
                    Quantity = 20,
                    Offer = new Offer { Price = 10 }
                }
            };


            decimal expected = 4200;


            //Act
            decimal actual = _cartService.CalculateItemsTotal(cartItems);

            //Assert
            actual.Should().Be(expected);
        }

        [Fact]
        public void CalculateItemsTotal_NullList_ReturnsZero()
        {
            //Arrange
            IEnumerable<CartItem> cartItems = null;

            //Act
            decimal expected = 0;
            decimal actual = _cartService.CalculateItemsTotal(cartItems);

            //Assert
            actual.Should().Be(expected);
        }

        [Fact]
        public void CalculateItemsTotal_EmptyList_ReturnsZero()
        {
            //Arrange
            IEnumerable<CartItem> cartItems = new List<CartItem>();

            //Act
            decimal expected = 0;
            decimal actual = _cartService.CalculateItemsTotal(cartItems);

            //Assert
            actual.Should().Be(expected);
        }
        #endregion

        #region CalculateMinimalDeliveryCost Method Tests
        [Fact]
        public void CalculateMinimalDeliveryCost_ItemsWithDeliveryTypes_ReturnsSumOfMinimalPrices()
        {
            //Arrange
            DeliveryType deliveryType1 = _fixture.Build<DeliveryType>().With(item => item.Price, 12).Create();
            DeliveryType deliveryType2 = _fixture.Build<DeliveryType>().With(item => item.Price, 11).Create();
            DeliveryType deliveryType3 = _fixture.Build<DeliveryType>().With(item => item.Price, 69).Create();

            Offer offer1 = _fixture.Build<Offer>().With(item => item.OfferDeliveryTypes, new List<OfferDeliveryType>()
            {
                 new OfferDeliveryType(){ DeliveryType = deliveryType1 },
                 new OfferDeliveryType(){ DeliveryType = deliveryType2 },
            })
            .Create();

            Offer offer2 = _fixture.Build<Offer>().With(item => item.OfferDeliveryTypes, new List<OfferDeliveryType>()
            {
                 new OfferDeliveryType(){ DeliveryType = deliveryType3 },
            })
            .Create();

            IEnumerable<CartItem> cartItems = new List<CartItem>()
            {
                new CartItem()
                {
                    Quantity = 15,
                    Offer = offer1,
                },
                new CartItem()
                {
                    Quantity = 1,
                    Offer = offer2,
                },
            };

            decimal expected = 11 + 69;

            //Act
            decimal actual = _cartService.CalculateMinimalDeliveryCost(cartItems);

            //Assert
            actual.Should().Be(expected);
        }

        [Fact]
        public void CalculateMinimalDeliveryCost_EmptyList_ReturnsZero()
        {
            // Arrange
            IEnumerable<CartItem> cartItems = new List<CartItem>();

            // Act
            decimal result = _cartService.CalculateMinimalDeliveryCost(cartItems);

            // Assert
            Assert.Equal(0, result);
        }

        [Fact]
        public void CalculateMinimalDeliveryCost_NullList_ReturnsZero()
        {
            // Arrange
            IEnumerable<CartItem> cartItems = null;

            // Act
            decimal result = _cartService.CalculateMinimalDeliveryCost(cartItems);

            // Assert
            Assert.Equal(0, result);
        }
        #endregion
    }
}
