using Cyberbit.TaskManager.Server.Models;
using Cyberbit.TaskManager.Server.Models.Enums;

namespace Cyberbit.TaskManager.Server.AuthorizeHelpers
{
    public class AuthenticateResponse
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public UserRole Role { get; set; }

        public AuthenticateResponse(User user, string token)
        {
            Id = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;
            Role = user.Role;
            Token = token;
        }
    }
}