using ContactsBook.Models;
using RecruitmentTask.Entities;
using RecruitmentTask.Models;

namespace RecruitmentTask.Services
{
    public interface IContactService
    {
        IEnumerable<GetContactsDto> GetAll();
        Contact GetById(int id);
        bool Remove(int id);
        void Add(CreateContactDto createContactDto);
        bool Update(UpdateContactDto updateContactDto, int id);
    }
}
