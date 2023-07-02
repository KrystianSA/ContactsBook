using System.ComponentModel.DataAnnotations;

namespace RecruitmentTask.Entities
{
    public class Contact
    {
        public int Id { get; set; }
        
        [Required]
        public string Name { get; set; }
        
        [Required]
        public string Surname { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        public double PhoneNumber { get; set; }
        
        [Required]
        [Range(1, 3, ErrorMessage = "CategoryId musi być wartością między 1 a 3.")]
        public int CategoryId { get; set; }
    }
}
