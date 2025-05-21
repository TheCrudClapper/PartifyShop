using ComputerServiceOnlineShop.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSOS.Core.Domain.RepositoryContracts
{
    public interface IProductImageRepository
    {
        Task<List<ProductImage>> GetImagesFromOffer(int offerId, List<string> imageUrls);
    }
}
