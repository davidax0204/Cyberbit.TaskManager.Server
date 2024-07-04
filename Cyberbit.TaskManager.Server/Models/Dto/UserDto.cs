using Cyberbit.TaskManager.Server.Models.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Cyberbit.TaskManager.Server.Models.Dto
{
    public class UserDto
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public UserRole Role { get; set; }
    }
}
