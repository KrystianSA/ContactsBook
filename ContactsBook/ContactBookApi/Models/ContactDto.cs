using RecruitmentTask.Entities;

namespace ContactsBook.Models
{
    public class ContactDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public double PhoneNumber { get; set; }
        public string CategoryName { get; set; }
    }
}
