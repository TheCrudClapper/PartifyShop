using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSOS.Core.DTO.Responses.Offers
{
    public class OfferBrowserItemResponseDto
    {
        public int Id { get; set; }
        public string Condition { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public string Category { get; set; }
        public DateTime DateCreated { get; set; }
        public string Description { get; set; }
        public int QuantityAvailable { get; set; }
        public decimal Price { get; set; }
        public string SellerName { get; set; }
    }
}
