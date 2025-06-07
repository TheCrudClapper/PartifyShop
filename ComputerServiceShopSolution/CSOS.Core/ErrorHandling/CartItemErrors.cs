using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSOS.Core.ErrorHandling
{
    public static class CartItemErrors
    {
        public static readonly Error CartItemDoesNotExists = new Error(
            "CartItem.ItemDoesNotExists", "Item of given id does not exists");

        public static readonly Error QuantityLowerThanZero = new Error(
            "CartItem.QuantityLowerThanZero", "Quantity must be greater that zero !");

        public static readonly Error CannotAddMoreToCartException = new Error(
            "CartItem.CannotAddMoreToCartException", "Cannot add more than is in shop");

        public static readonly Error InvalidItemQuantity = new Error(
            "CartItem.InvalidItemQuantity", "Invalid item quantity");
    }
}
