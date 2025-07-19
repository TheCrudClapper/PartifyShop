namespace CSOS.Core.ErrorHandling
{
    public static class CartErrors
    {
        public static Error CartDoesNotExist(int id) => new Error(
            "Cart.DoesNotExist", $"Cart with id {id} doesnt exist");

        public static Error CartDoesNotExists = new Error(
            "Cart.DoesNotExist", $"Cart for given user does not exist");
    }
}
