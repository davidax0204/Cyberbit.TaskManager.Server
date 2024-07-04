using Cyberbit.TaskManager.Server.Interfaces;
using Cyberbit.TaskManager.Server.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cyberbit.TaskManager.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ILogger<TasksController> _logger;
        private readonly ITasksBl _TasksBl;
        private readonly IAutoMapperService _autoMapper;

        public TasksController(ILogger<TasksController> logger, ITasksBl tasksBl, IAutoMapperService autoMapper)
        {
            _logger = logger;
            _TasksBl = tasksBl;
            _autoMapper = autoMapper;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<TaskDto>>> GetAllTasks()
        {
            var allTasks = await _TasksBl.GetAllTask();
            if (allTasks == null)
            {
                var errMsg = "Failed to get all Tasks";
                _logger.LogError(errMsg);
                return BadRequest(errMsg);
            }
            var allTasksDto = _autoMapper.Mapper.Map<List<TaskDto>>(allTasks);
            return allTasksDto;
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<TaskDto>> GetTaskById([FromRoute] int id)
        {
            if (id <= 0)
            {
                var errMsg = $"Failed to get Task with invalid id '{id}'";
                _logger.LogError(errMsg);
                return BadRequest(errMsg);
            }

            var Task = await _TasksBl.GetTaskById(id);
            if (Task == null)
            {
                var errMsg = $"Failed to get Task with id '{id}. Task not found'";
                _logger.LogError(errMsg);
                return NotFound(errMsg);
            }
            var TaskDto = _autoMapper.Mapper.Map<TaskDto>(Task);
            return TaskDto;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<TaskDto>> AddTask([FromBody] TaskDto taskDto)
        {
            var currentUser = (int)ControllerContext.HttpContext.Items["UserId"];
            if (taskDto == null)
            {
                var errMsg = $"Input Task dto model is invalid";
                _logger.LogError(errMsg);
                return BadRequest(errMsg);
            }

            var task = _autoMapper.Mapper.Map<Models.Task>(taskDto);
            var addedTask = await _TasksBl.AddTask(task, currentUser);
            if (addedTask == null)
            {
                var errMsg = $"Failed to add Task. Check logs";
                _logger.LogError(errMsg);
                return BadRequest(errMsg);
            }

            var addedTaskDto = _autoMapper.Mapper.Map<TaskDto>(addedTask);
            return addedTaskDto;
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<TaskDto>> UpdateTask([FromRoute] int id, [FromBody] TaskDto taskDto)
        {
            if (id <= 0)
            {
                var errMsg = $"Failed to update Task with invalid id '{id}'";
                _logger.LogError(errMsg);
                return BadRequest(errMsg);
            }

            if (id != taskDto.Id)
            {
                var errMsg = $"Failed to update Task. Id in url and in dto are mistmatched";
                _logger.LogError(errMsg);
                return BadRequest(errMsg);
            }

            var Task = _autoMapper.Mapper.Map<Models.Task>(taskDto);
            Models.Task updatedTask = null;
            try
            {
                updatedTask = await _TasksBl.UpdateTask(Task);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception occurred while trying to updated Task '{id}'");
            }

            if (updatedTask == null)
            {
                var errMsg = $"Failed to update Task with id '{id}'. Check logs";
                _logger.LogError(errMsg);
                return BadRequest(errMsg);
            }

            var updatedTaskDto = _autoMapper.Mapper.Map<TaskDto>(updatedTask);
            return updatedTaskDto;
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<TaskDto>> DeleteTaskById([FromRoute] int id)
        {
            if (id <= 0)
            {
                var errMsg = $"Failed to delete Task with invalid id '{id}'";
                _logger.LogError(errMsg);
                return BadRequest(errMsg);
            }

            Models.Task Task = null;
            try
            {
                Task = await _TasksBl.DeleteTaskById(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception occurred while trying to delete Task '{id}'");
            }

            if (Task == null)
            {
                var errMsg = $"Failed to delete Task with id '{id}'. Check logs";
                _logger.LogError(errMsg);
                return BadRequest(errMsg);
            }

            var deletedTaskDto = _autoMapper.Mapper.Map<TaskDto>(Task);
            return deletedTaskDto;
        }

        [Authorize]
        [HttpPut("doneAll/{employeeId}")]
        public async Task<ActionResult<IList<TaskDto>>> MarkAllTasksAsDone([FromRoute] int employeeId)
        {
            if (employeeId <= 0)
            {
                var errMsg = $"Failed to Complete all Task with invalid id '{employeeId}'";
                _logger.LogError(errMsg);
                return BadRequest(errMsg);
            }

            IList<Models.Task> task = null;
            try
            {
                task = await _TasksBl.CompleteAllTasks(employeeId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Exception occurred while trying to complete all tasks for employee '{employeeId}'");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }

            if (task == null || !task.Any())
            {
                var errMsg = $"Failed to mark tasks as done for employee '{employeeId}'. No tasks found.";
                _logger.LogError(errMsg);
                return NotFound(errMsg);
            }

            var taskDtos = _autoMapper.Mapper.Map<IList<TaskDto>>(task);
            return Ok(taskDtos);
        }
    }
}
