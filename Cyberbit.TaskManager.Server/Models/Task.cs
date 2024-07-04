using Cyberbit.TaskManager.Server.Models.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.ComponentModel.DataAnnotations;

namespace Cyberbit.TaskManager.Server.Models
{
    public class Task
    {
        [Key, Required]
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string Title { get; set; }
        [Required, MaxLength(250)]
        public string Description { get; set; }
        public int CreatedByUserId { get; set; }
        public User CreatedByUser { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime DueDate { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public TasksStatus Status { get; set; }

    }
}
