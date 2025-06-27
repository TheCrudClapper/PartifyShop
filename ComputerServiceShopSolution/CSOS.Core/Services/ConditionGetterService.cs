using CSOS.Core.Domain.RepositoryContracts;
using CSOS.Core.DTO;
using CSOS.Core.ServiceContracts;

namespace CSOS.Core.Services
{
    public class ConditionGetterService : IConditionGetterService
    {

        public readonly IConditionRepository _conditionRepo;
        public ConditionGetterService(IConditionRepository conditionRepository)
        {
            _conditionRepo = conditionRepository;
        }
        public async Task<List<SelectListItemDto>> GetProductConditionsAsSelectList()
        {
            var conditions = await _conditionRepo.GetAllConditionsAsync();

            return conditions.Select(item => new SelectListItemDto()
            {
                Text = item.ConditionTitle,
                Value = item.Id + "",
            })
            .ToList();
        }

    }
}
