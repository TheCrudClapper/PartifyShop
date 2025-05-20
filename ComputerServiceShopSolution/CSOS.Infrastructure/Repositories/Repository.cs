using ComputerServiceOnlineShop.Entities.Contexts;
using ComputerServiceOnlineShop.Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace CSOS.Infrastructure.Repositories
{
    public abstract class Repository<TEntity>
        where TEntity : BaseModel
    {
        protected readonly DatabaseContext DbContext;
        protected Repository(DatabaseContext dbContext)
        {
            DbContext = dbContext;
        }

        public void Add(TEntity entity)
        {
            DbContext.Set<TEntity>().Add(entity);
        }
        public void Update(TEntity entity)
        {
            DbContext.Set<TEntity>().Update(entity);
        }
        public void Remove(TEntity entity)
        {
            entity.IsActive = false;
            entity.DateDeleted = DateTime.Now;
        }
    }
}
