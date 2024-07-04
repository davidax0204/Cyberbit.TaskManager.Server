using System.ComponentModel.DataAnnotations;

namespace Cyberbit.TaskManager.Server.Models
{
    public class Category
    {
        [Key, Required]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        public bool IsDeleted { get; set; }
    }
}
