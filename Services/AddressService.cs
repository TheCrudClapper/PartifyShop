using ComputerServiceOnlineShop.Entities.Contexts;
using ComputerServiceOnlineShop.ServiceContracts;
using Microsoft.AspNetCore.Mvc;

namespace ComputerServiceOnlineShop.Services
{
    public class AddressService : IAddressService
    {
        private readonly DatabaseContext _databaseContext;
        public AddressService(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }
        public Task<IActionResult> Edit(int id)
        {
            throw new NotImplementedException();
        }
    }
}
