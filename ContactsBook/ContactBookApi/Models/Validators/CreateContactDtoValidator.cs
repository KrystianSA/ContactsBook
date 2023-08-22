using FluentValidation;
using Microsoft.EntityFrameworkCore;
using RecruitmentTask.Data;
using RecruitmentTask.Entities;

namespace ContactsBook.Models.Validators
{
    public class CreateContactDtoValidator : ValidationBaseForUpdateAndCreateContact<CreateOrUpdateContactDto>
    {
        private readonly DataDbContext _dbContext;

        public CreateContactDtoValidator(DataDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        protected  void ValidateRules() 
        {
            RuleFor(email => email.Email)
                .Must(IsEmailExist).WithMessage("Email already exist");
        }

        private bool IsEmailExist(string email)
        {
            var isEmailExist = _dbContext.Contacts.SingleOrDefault(c => c.Email == email);

            if(isEmailExist!=null) 
            {
                return false;
            }

            return true;
        }
    }
}

