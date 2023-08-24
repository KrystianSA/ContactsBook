using RecruitmentTask.Entities;
using System.Text.Json.Serialization;

namespace ContactsBook.Models
{
    public class GetContactsDto : ContactBaseDto
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
    }
}
