using AutoMapper;
using Azure.Messaging;
using ContactsBook.Models;
using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using RecruitmentTask.Data;
using RecruitmentTask.Entities;

namespace RecruitmentTask.Services
{
    public class ContactService : IContactService
    {
        private readonly DataDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateContactDto> _createContacValidator;

        public ContactService(DataDbContext dbContext,IMapper mapper, IValidator<CreateContactDto> createContacValidator)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _createContacValidator = createContacValidator;
        }

        public IEnumerable<GetContactsDto> GetAll()
        {
            var contacts = _dbContext.Contacts.Include(x=>x.Category).ToList();
            var contactDtos = _mapper.Map<IEnumerable<GetContactsDto>>(contacts);
            return contactDtos;
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

        public void Create(CreateContactDto createContactDto)
        {

            var existingContact = _dbContext.Contacts.SingleOrDefault(x => x.Email == createContactDto.Email);
            if (existingContact != null)
            {
                throw new Exception("Email already exists");
            }
            
            var validateContactToCreate = _createContacValidator.Validate(createContactDto);

            if(!validateContactToCreate.IsValid) 
            {
                throw new Exception("Problem with data, which try add to database");
            }

            var contact = _mapper.Map<Contact>(createContactDto);
            _dbContext.Contacts.Add(contact);
            _dbContext.SaveChanges();
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
