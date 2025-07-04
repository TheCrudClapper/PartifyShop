﻿namespace CSOS.Core.ErrorHandling
{
    public class AddressErrors
    {
        public static readonly Error AddressNotFound = new Error(
            "Address.AddressNotFound", "Address of given id not found");

        public static readonly Error MissingAddressData = new Error (
            "Address.MissingAddressData", "User's address details could not be loaded.");

    }
}
