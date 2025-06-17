using ComputerServiceOnlineShop.Entities.Models;
using ComputerServiceOnlineShop.ServiceContracts;
using ComputerServiceOnlineShop.Services;
namespace CSOS.Tests
{
    public class CartServiceTests
    {
        private ICartService _cartService;
        public CartServiceTests()
        {
            //should be mocked
            _cartService = new CartService();
        }

        #region CalculateItemsTotal Method Testss
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
            Assert.Equal(expected, actual);
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
            Assert.Equal(expected, actual);
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
            Assert.Equal(expected, actual);
        }
        #endregion

        #region CalculateMinimalDeliveryCost Method Tests
        [Fact]
        public void CalculateMinimalDeliveryCost_ItemsWithDeliveryTypes_ReturnsSumOfMinimalPrices()
        {
            //Arrange
            DeliveryType deliveryType1 = new DeliveryType { Price = 12 };
            DeliveryType deliveryType2 = new DeliveryType { Price = 11 };
            DeliveryType deliveryType3 = new DeliveryType { Price = 69 };

            Offer offer1 = new Offer()
            {
                OfferDeliveryTypes = new List<OfferDeliveryType>
                {
                    new OfferDeliveryType(){ DeliveryType = deliveryType1 },
                    new OfferDeliveryType(){ DeliveryType = deliveryType2 },
                }
            };

            Offer offer2 = new Offer()
            {
                OfferDeliveryTypes = new List<OfferDeliveryType>
                {
                    new OfferDeliveryType(){ DeliveryType = deliveryType3 },
                }
            };


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
            Assert.True(expected == actual);
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
