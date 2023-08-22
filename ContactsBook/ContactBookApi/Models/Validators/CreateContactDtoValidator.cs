using FluentValidation;
using Microsoft.EntityFrameworkCore;
using RecruitmentTask.Data;
using RecruitmentTask.Entities;

namespace ContactsBook.Models.Validators
{
    public class CreateContactDtoValidator : AbstractValidator<CreateContactDto>
    {
        private readonly DataDbContext _dbContext;

        public CreateContactDtoValidator(DataDbContext dbContext)
        {
            _dbContext = dbContext;
            ValidateRules();
        }

        private void ValidateRules()
        {
            RuleFor(name => name.Name)
               .NotEmpty();

            RuleFor(surname => surname.Surname)
                .NotEmpty();

            RuleFor(email => email.Email)
                .EmailAddress()
                .Must(IsEmailExist).WithMessage("Email already exist");

            RuleFor(phoneNumber => phoneNumber.PhoneNumber)
                .GreaterThanOrEqualTo(9);

            RuleFor(categoryId => categoryId.CategoryId)
                .InclusiveBetween(1, 3)
                .WithMessage(" 1 - Private\n 2 - Business\n 3 - Other ");
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

