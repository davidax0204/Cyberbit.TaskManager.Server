using Cyberbit.TaskManager.Server.Interfaces;
using Cyberbit.TaskManager.Server.Models;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cyberbit.TaskManager.Server.Bl
{
    public class CategoriesBl : ICategoriesBl
    {
        private readonly ILogger<CategoriesBl> _logger;
        private readonly ICategoriesDal _categoriesDal;

        public CategoriesBl(ILogger<CategoriesBl> logger, ICategoriesDal categoriesDal)
        {
            _logger = logger;
            _categoriesDal = categoriesDal;
        }

        public async Task<IList<Category>> GetAllCategories()
        {
            _logger.LogInformation($"GetAllCategory - Enter");
            var retValue = await _categoriesDal.GetAllCategories();
            _logger.LogInformation($"GetAllCategory - Exit");
            return retValue;
        }

        public async Task<Category> AddCategory(Category category)
        {
            _logger.LogInformation($"AddCategory - Enter");
            var retValue = await _categoriesDal.AddCategory(category);
            _logger.LogInformation($"AddCategory - Exit");
            return retValue;
        }

        public async Task<Category> UpdateCategory(Category category)
        {
            _logger.LogInformation($"UpdateCategory - Enter");
            var retValue = await _categoriesDal.UpdateCategory(category);
            _logger.LogInformation($"UpdateCategory - Exit");
            return retValue;
        }

        public async Task<Category> DeleteCategory(int id)
        {
            _logger.LogInformation($"DeleteCategory - Enter");
            var retValue = await _categoriesDal.DeleteCategory(id);
            _logger.LogInformation($"DeleteCategory - Exit");
            return retValue;
        }
    }
}
