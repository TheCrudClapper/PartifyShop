namespace CSOS.Core.ErrorHandling
{
    public class AddressErrors
    {
        public static readonly Error AddressNotFound = new Error(
            "Address.AddressNotFound", "Address of given id not found");
    }
}
