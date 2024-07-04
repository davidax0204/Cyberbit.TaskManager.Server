using Cyberbit.TaskManager.Server.Models.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cyberbit.TaskManager.Server.Models
{
    public class User
    {
        [Key, Required]
        public int Id { get; set; }

        [Required, EmailAddress, MaxLength(100)]
        public string Email { get; set; }

        [Required, MaxLength(12)]
        public string Password { get; set; }

        [Required, MaxLength(50)]
        public string FirstName { get; set; }

        [Required, MaxLength(50)]
        public string LastName { get; set; }

        public bool IsDeleted { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public UserRole Role { get; set; }

        public List<Task> Tasks { get; set; }
        public List<Task> CreatedByTasks { get; set; }
        public DateTime CreateTime { get; set; }
    }
}
