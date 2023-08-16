using RecruitmentTask.Entities;
using RecruitmentTask.Models;

namespace RecruitmentTask.Services
{
    public interface IContactService
    {
        IEnumerable<Contact> GetAll();
        Contact GetById(int id);
        bool Remove(int id);
        Contact Add(Contact user);
        bool Update(Contact user, int id);
    }
}
