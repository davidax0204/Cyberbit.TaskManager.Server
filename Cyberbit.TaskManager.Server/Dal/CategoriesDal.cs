using Cyberbit.TaskManager.Server.Interfaces;
using Cyberbit.TaskManager.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cyberbit.TaskManager.Server.Dal
{
    public class CategoriesDal : ICategoriesDal
    {
        private readonly ILogger<CategoriesDal> _logger;
        private readonly BackendDbContext _dbContext;

        public CategoriesDal(ILogger<CategoriesDal> logger, BackendDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<IList<Category>> GetAllCategories()
        {
            _logger.LogInformation($"GetAllCategories - Enter");
            var retValue = await _dbContext.Categories.AsNoTracking()
                .Where(c => !c.IsDeleted)
                .ToListAsync();
            _logger.LogInformation($"GetAllCategories - Exit");
            return retValue;
        }

        public async Task<Category> AddCategory(Category category)
        {
            _logger.LogInformation($"AddCategory - Enter");
            var addedEntity = _dbContext.Categories.Add(category);
            await _dbContext.SaveChangesAsync();
            addedEntity.State = EntityState.Detached;
            _logger.LogInformation($"AddCategory - Exit");
            return addedEntity.Entity;
        }

        public async Task<Category> UpdateCategory(Category category)
        {
            _logger.LogInformation($"UpdateCategory - Enter");
            var updatedEntity = _dbContext.Categories.Update(category);
            await _dbContext.SaveChangesAsync();
            updatedEntity.State = EntityState.Detached;
            _logger.LogInformation($"UpdateCategory - Exit");
            return updatedEntity.Entity;
        }
        
        public async Task<Category> DeleteCategory(int id)
        {
            _logger.LogInformation($"DeleteCategory - Enter");
            var categoryToDelete = _dbContext.Categories.Single(c => c.Id == id);
            categoryToDelete.IsDeleted = true;
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation($"DeleteCategory - Exit");
            return categoryToDelete;
        }
    }
}
