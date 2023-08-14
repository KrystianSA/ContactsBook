using RecruitmentTask.Entities;

namespace ContactsBook.Models
{
    public class Contact
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public double PhoneNumber { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
    }
}
