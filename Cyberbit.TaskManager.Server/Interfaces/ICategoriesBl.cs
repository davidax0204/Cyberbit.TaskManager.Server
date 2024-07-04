using Cyberbit.TaskManager.Server.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cyberbit.TaskManager.Server.Interfaces
{
    public interface ICategoriesBl
    {
        Task<IList<Category>> GetAllCategories();

        Task<Category> AddCategory(Category category);

        Task<Category> UpdateCategory(Category category);

        Task<Category> DeleteCategory(int id);
    }
}
