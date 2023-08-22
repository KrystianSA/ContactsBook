﻿using ContactsBook.Models;
using RecruitmentTask.Entities;
using RecruitmentTask.Models;

namespace RecruitmentTask.Services
{
    public interface IContactService
    {
        IEnumerable<GetContactsDto> GetAll();
        Contact GetById(int id);
        bool Remove(int id);
        void Add(CreateOrUpdateContactDto createContactDto);
        bool Update(CreateOrUpdateContactDto updateContactDto, int id);
    }
}
