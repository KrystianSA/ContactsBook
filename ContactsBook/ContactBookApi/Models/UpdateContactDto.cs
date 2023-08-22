namespace ContactsBook.Models
{
    public class UpdateContactDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public double PhoneNumber { get; set; }
        public int CategoryId { get; set; }
    }
}
