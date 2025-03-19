using ComputerServiceOnlineShop.Models.Contexts;

namespace ComputerServiceOnlineShop.Models.Services
{
    public class BaseService
    {
        public DatabaseContext DatabaseContext { get; }
        public BaseService(DatabaseContext databaseContext)
        {
            DatabaseContext = databaseContext;
        }
    }
}
