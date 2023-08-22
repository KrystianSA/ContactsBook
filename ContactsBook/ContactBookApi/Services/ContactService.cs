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
        private readonly IValidator<CreateOrUpdateContactDto> _createContacValidator;
        private readonly IValidator<CreateOrUpdateContactDto> _updateContactValidator;

        public ContactService(DataDbContext dbContext, IMapper mapper, IValidator<CreateOrUpdateContactDto> createContacValidator, IValidator<CreateOrUpdateContactDto> updateContactValidator)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _createContacValidator = createContacValidator;
            _updateContactValidator = updateContactValidator;
        }

        public IEnumerable<GetContactsDto> GetAll()
        {
            var contacts = _dbContext.Contacts.Include(x => x.Category).ToList();
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

        public void Add(CreateOrUpdateContactDto createContactDto)
        {
            var validateContactToCreate = _createContacValidator.Validate(createContactDto);

            if (!validateContactToCreate.IsValid)
            {
                throw new Exception("Problem with data, which try add to database");
            }

            var contact = _mapper.Map<Contact>(createContactDto);
            _dbContext.Contacts.Add(contact);
            _dbContext.SaveChanges();
        }

        public bool Update(CreateOrUpdateContactDto updateContactDto, int id)
        {
            var contactToUpdate = _dbContext.Contacts.SingleOrDefault(x => x.Id == id);

            if (contactToUpdate is null)
            {
                return false;
            }

            if (!_updateContactValidator.Validate(updateContactDto).IsValid)
            {
                return false;
            }
            contactToUpdate.Name = updateContactDto.Name;
            contactToUpdate.Surname = updateContactDto.Surname;
            contactToUpdate.Email = updateContactDto.Email;
            contactToUpdate.PhoneNumber = updateContactDto.PhoneNumber;
            contactToUpdate.CategoryId = updateContactDto.CategoryId;

            _dbContext.SaveChanges();

            return true;
        }
    }
}
