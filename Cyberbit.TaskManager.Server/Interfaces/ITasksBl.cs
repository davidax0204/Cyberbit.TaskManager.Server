using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cyberbit.TaskManager.Server.Interfaces
{
    public interface ITasksBl
    {
        Task<IList<Models.Task>> GetAllTask();

        Task<Models.Task> GetTaskById(int id);

        Task<Models.Task> AddTask(Models.Task task, int createdByUserId);

        Task<Models.Task> UpdateTask(Models.Task task);

        Task<Models.Task> DeleteTaskById(int id);
        Task<IList<Models.Task>> CompleteAllTasks(int employeeId);
    }
}
