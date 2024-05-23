using System.ComponentModel.DataAnnotations;

namespace OrangeBranchTaskManager.Api.DTOs
{
    public class RegisterDTO
    {
        [Required]
        [StringLength(100)]
        public string? Username { get; set; }

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [StringLength(100)]
        public string? Password { get; set; }
    }
}
