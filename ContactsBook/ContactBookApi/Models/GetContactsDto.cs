using RecruitmentTask.Entities;

namespace ContactsBook.Models
{
    public class GetContactsDto : ContactBaseDto
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
    }
}
