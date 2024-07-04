using Cyberbit.TaskManager.Server.Models.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;

namespace Cyberbit.TaskManager.Server.Models.Dto
{
    public class TaskDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int CreatedByUserId { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime DueDate { get; set; }
        public int UserId { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public TasksStatus Status { get; set; }

    }
}
