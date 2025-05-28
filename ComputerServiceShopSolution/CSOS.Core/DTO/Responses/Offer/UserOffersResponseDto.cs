using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSOS.Core.DTO.Responses.Offers
{
    public class UserOffersResponseDto
    {
        public int Id { get; set; }
        public string? ImageUrl { get; set; } = null!;
        public string ProductName { get; set; } = null!;
        public string ProductCategory { get; set; } = null!;
        public string ProductCondition { get; set; } = null!;
        public bool ProductStatus { get; set; }

        public int StockQuantity;
        public decimal Price;
        public DateTime DateCreated { get; set; }
    }
}
