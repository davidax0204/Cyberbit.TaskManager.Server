using System.ComponentModel.DataAnnotations;

namespace Cyberbit.TaskManager.Server.AuthorizeHelpers
{
    public class AuthenticateRequest
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}