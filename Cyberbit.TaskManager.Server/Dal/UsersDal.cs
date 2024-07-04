using Cyberbit.TaskManager.Server.Interfaces;
using Cyberbit.TaskManager.Server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cyberbit.TaskManager.Server.Dal
{
    public class UsersDal : IUsersDal
    {
        private readonly ILogger<UsersDal> _logger;
        private readonly BackendDbContext _dbContext;

        public UsersDal(ILogger<UsersDal> logger, BackendDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        public async Task<IList<User>> GetAllUser()
        {
            _logger.LogInformation($"GetAllUser - Enter");
            var retValue = await _dbContext.Users.AsNoTracking().ToListAsync();
            _logger.LogInformation($"GetAllUser - Exit");
            return retValue;
        }
        
        public async Task<User> GetUserById(int id)
        {
            _logger.LogInformation($"GetUserById - Enter");
            var retValue = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Id == id);
            _logger.LogInformation($"GetUserById - Exit");
            return retValue;
        }
        
        public async Task<User> AddUser(User user)
        {
            _logger.LogInformation($"AddUser - Enter");
            user.CreateTime = DateTime.Now;
            var addedEntity = _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
            addedEntity.State = EntityState.Detached;
            _logger.LogInformation($"AddUser - Exit");
            return addedEntity.Entity;
        }

        public async Task<User> UpdateUser(User user)
        {
            _logger.LogInformation($"UpdateUser - Enter");
            var updatedEntity = _dbContext.Users.Update(user);
            _dbContext.Entry(user).Property(x => x.CreateTime).IsModified = false;
            _dbContext.Entry(user).Property(x => x.Password).IsModified = false;
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation($"UpdateUser - Exit");
            return updatedEntity.Entity;
        }
        
        public async Task<User> DeleteUserById(int id)
        {
            _logger.LogInformation($"DeleteUserById - Enter");
            var userToDelete = new User { Id = id };
            var deletedEntity = _dbContext.Users.Remove(userToDelete);
            await _dbContext.SaveChangesAsync();
            _logger.LogInformation($"DeleteUserById - Exit");
            return deletedEntity.Entity;
        }

        public Task<User> GetUser(string userEmail, string password)
        {
            _logger.LogInformation($"GetUserById - Enter");
            return _dbContext.Users.FirstOrDefaultAsync(user => user.Email.ToLower() == userEmail.ToLower() && user.Password == password);
        }
    }
}
