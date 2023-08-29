using ContactsBook.Models.Validators.Extensions;
using FluentValidation;
using System.Text.RegularExpressions;

namespace ContactsBook.Models.Validators
{
    public abstract class ValidationBaseForUpdateAndCreateContact<T> : AbstractValidator<T> where T : CreateOrUpdateContactDto
    {
        public ValidationBaseForUpdateAndCreateContact()
        {
            ValidateRules();
        }
        protected virtual void ValidateRules()
        {
            RuleFor(name => name.Name)
               .NotEmpty();

            RuleFor(surname => surname.Surname)
                .NotEmpty();

            RuleFor(email => email.Email)
                .EmailAddress();

            RuleFor(phoneNumber => phoneNumber.PhoneNumber)
                .Must(numberToString=>numberToString.ToString().IsNumberCorrect())
                .WithMessage("Invalid phone phoneNumber");

            RuleFor(categoryId => categoryId.CategoryId)
                .InclusiveBetween(1, 3)
                .WithMessage(" 1 - Private\n 2 - Business\n 3 - Other ");
        }
    }
}
