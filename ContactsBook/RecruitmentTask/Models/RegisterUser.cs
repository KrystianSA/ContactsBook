using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;

namespace RecruitmentTask.Models
{
    public class RegisterUser
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public double PhoneNumber { get; set; }
        [Required]

        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
    }
}