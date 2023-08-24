using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace ContactsBook.Models
{
    public abstract class ContactBaseDto
    { 
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public double PhoneNumber { get; set; }
    }
}
