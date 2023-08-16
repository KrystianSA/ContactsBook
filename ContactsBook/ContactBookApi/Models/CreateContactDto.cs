using RecruitmentTask.Entities;

namespace ContactsBook.Models
{
    public class CreateContactDto
    { 
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public double PhoneNumber { get; set; }
        public int CategoryId { get; set; }
    }
}
