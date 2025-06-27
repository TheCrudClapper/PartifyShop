using AutoFixture;
using ComputerServiceOnlineShop.Entities.Models;
using ComputerServiceOnlineShop.ServiceContracts;
using ComputerServiceOnlineShop.Services;
using FluentAssertions;
namespace CSOS.Tests
{
    public class CartServiceTests
    {
        private readonly ICartService _cartService;
        private readonly IFixture _fixture;
        public CartServiceTests()
        {
            //should be mocked
            _cartService = new CartService();
            _fixture = new Fixture();

            _fixture.Behaviors
            .OfType<ThrowingRecursionBehavior>()
            .ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));

            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

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
