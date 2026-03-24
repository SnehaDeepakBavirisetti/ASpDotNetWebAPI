using System.ComponentModel.DataAnnotations;

namespace ASpDotNetWebAPI.Models
{
    public class EmployeeDTO
    {
        [Required]
        [MaxLength(20)]
        public required string Name { get; set; }
        [Required]
        [MaxLength(2)]
        public required string Age { get; set; }
        [Required]
        [MaxLength(15)]
        public required string Email { get; set; }
        [Required]
        [MaxLength(10)]
        public required string Phone { get; set; }
    }
}