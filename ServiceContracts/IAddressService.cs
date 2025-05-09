using Microsoft.AspNetCore.Mvc;

namespace ComputerServiceOnlineShop.ServiceContracts
{
    public interface IAddressService
    {
        /// <summary>
        /// Edits address of user
        /// </summary>
        /// <param name="id">id of address to edit</param>
        /// <returns></returns>
        Task<IActionResult> Edit(int id);
    }
}
