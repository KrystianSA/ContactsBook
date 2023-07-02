using AutoMapper;
using Azure.Messaging;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.OpenApi.Validations;
using RecruitmentTask.Data;
using RecruitmentTask.Entities;
using RecruitmentTask.Models;

namespace RecruitmentTask.Services
{
    public class ContactService : IContactService
    {
        private readonly DataDbContext _dbContext;
        private readonly IMapper _mapper;

        public ContactService(DataDbContext dbContext,IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public IEnumerable<Contact> GetAll()
        {
            return _dbContext.Contacts.ToList();
        }

        public Contact GetById(int id)
        {
            var contact = _dbContext.Contacts.SingleOrDefault(x => x.Id == id);
            return contact;
        }

        public bool Remove(int id)
        {
            var contact = _dbContext.Contacts.SingleOrDefault(x => x.Id == id);

            if (contact is null) return false;

            _dbContext.Contacts.Remove(contact);
            _dbContext.SaveChanges();

            return true;
        }

        public Contact Add(Contact contact)
        {

            var existingContact = _dbContext.Contacts.SingleOrDefault(x => x.Email == contact.Email);
            if (existingContact != null)
            {
                throw new Exception("Email already exists");
            }

            _dbContext.Contacts.Add(contact);
            _dbContext.SaveChanges();

            return contact;
        }

        public bool Update(Contact contact, int id)
        {
            var contactToUpdate = _dbContext.Contacts.SingleOrDefault(x => x.Id == id);

            if (contactToUpdate is null)
            {
                return false;
            }

            contactToUpdate.Name = contact.Name;
            contactToUpdate.Surname = contact.Surname;
            contactToUpdate.Email = contact.Email;
            contactToUpdate.PhoneNumber = contact.PhoneNumber;
            contactToUpdate.CategoryId = contact.CategoryId;

            _dbContext.SaveChanges();

            return true;
        }
    }
}
