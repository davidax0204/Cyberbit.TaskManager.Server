using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cyberbit.TaskManager.Server.Interfaces
{
    public interface ITasksDal
    {
        Task<IList<Models.Task>> GetAllTask();

        Task<Models.Task> GetTaskById(int id);

        Task<Models.Task> AddTask(Models.Task task);

        Task<Models.Task> UpdateTask(Models.Task task);

        Task<Models.Task> DeleteTaskById(int id);
        Task<IList<Models.Task>> CompleteAllTasks(int employeeId);
    }
}
