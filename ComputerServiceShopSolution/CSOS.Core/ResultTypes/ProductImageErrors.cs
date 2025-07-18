﻿namespace CSOS.Core.ResultTypes
{
    public static class ProductImageErrors
    {
        public static readonly Error ProductImageIsNull = new Error(
           "ProductImage.ProductImageIsNull", "Given offer doesnt have any images");

        public static readonly Error ProductImagesAreEmpty = new Error(
           "ProductImage.ProductImagesAreEmpty", "Product images are empty");
    }
}
