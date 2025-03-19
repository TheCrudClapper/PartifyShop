using ComputerServiceOnlineShop.Models.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ComputerServiceOnlineShop.Models.Services
{
    public class UserService : BaseService
    {
        public UserService(DatabaseContext databaseContext) : base(databaseContext)
        {
        }
        public List<User> GetAllUsers()
        {
            return DatabaseContext.Users.Where(item => item.IsActive).Include(item => item.Address).ThenInclude(item => item.Country).ToList();
        }
    }
}
