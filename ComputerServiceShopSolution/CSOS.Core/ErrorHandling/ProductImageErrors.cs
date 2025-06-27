using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSOS.Core.ErrorHandling
{
    public class ProductImageErrors
    {
        public static readonly Error ProductImageIsNull = new Error(
           "ProductImage.ProductImageIsNull", "Given offer doesnt have any images");

        public static readonly Error ProductImagesAreEmpty = new Error(
           "ProductImage.ProductImagesAreEmpty", "Product images are empty");
    }
}
