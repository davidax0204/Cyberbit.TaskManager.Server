using Cyberbit.TaskManager.Server.Models;
using Cyberbit.TaskManager.Server.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;

namespace Cyberbit.TaskManager.Server.Dal
{
    public class BackendDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Category> Categories { get; set; }

        public BackendDbContext(DbContextOptions<BackendDbContext> options)
        : base(options)
        {

        }

        #region FluentAPI
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

            modelBuilder.Entity<User>()
                .Property(u => u.Role)
                .HasConversion(new EnumToStringConverter<UserRole>());


            modelBuilder.Entity<Task>()
                .HasOne(t => t.CreatedByUser)
                .WithMany(u => u.CreatedByTasks)
                .HasForeignKey(f => f.CreatedByUserId);

            modelBuilder.Entity<Task>()
                .Property(t => t.Status)
                .HasConversion(new EnumToStringConverter<TasksStatus>());
        }
        #endregion

        public void SeedMockData()
        {
            Users.Add(new User
            {
                Id = 1,
                FirstName = "Admin",
                LastName = "1",
                Email = "admin_1@cyberbit.com",
                Role = UserRole.Manager,
                Password = "1234",
                CreateTime = DateTime.Now
            });

            for (int i = 1; i < 6; i++)
            {
                Users.Add(new User
                {
                    Id = i +1,
                    FirstName = "Employee",
                    LastName = i.ToString(),
                    Email = $"employee_{i}@cyberbit.com",
                    Role = UserRole.Employee,
                    Password = "1234",
                    CreateTime = DateTime.Now
                });
            }

            Categories.Add(new Category
            {
                Name = "Category A"
            });

            Categories.Add(new Category
            {
                Name = "Category B"
            });

            Categories.Add(new Category
            {
                Name = "Category C"
            });

            Categories.Add(new Category
            {
                Name = "Category D"
            });
            SaveChanges();

            Tasks.Add(new Task
            {
                CreatedByUserId = 1,
                CreationTime = DateTime.Now,
                Description = "Fix all bugs",
                DueDate = DateTime.Now.AddDays(7),
                Title = "Bugs",
                UserId = 2,
                Status = TasksStatus.Open
            });

            SaveChanges();
        }
    }
}
