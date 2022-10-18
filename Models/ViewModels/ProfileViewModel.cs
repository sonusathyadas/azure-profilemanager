using System.ComponentModel.DataAnnotations;

namespace ProfileManager.Models.ViewModels
{
    public class ProfileViewModel
    {
        [Required(ErrorMessage = "First name cannot be empty")]
        public string? FirstName { get; set; }

        [Required(ErrorMessage = "Last name cannot be empty")]
        public string? LastName { get; set; }

        [Required(ErrorMessage = "Designation cannot be empty")]
        public string? Designation { get; set; }

        [Required(ErrorMessage = "Join date cannot be empty")]
        [DataType(DataType.Date)]
        public DateTime JoinDate { get; set; }

        [Required(ErrorMessage = "Email cannot be empty")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Department cannot be empty")]
        public string? Department { get; set; }

        [DataType(DataType.Upload)]
        [Required(ErrorMessage ="Please select a file to upload")]
        public IFormFile? Profile { get; set; }
    }
}
