using FluentValidation;
using RecruitmentTask.Entities;

namespace ContactsBook.Models.Validators
{
    public class CreateContactDtoValidator : AbstractValidator<CreateContactDto>
    {
        public CreateContactDtoValidator()
        {
            RuleFor(name => name.Name)
                .NotNull();

            RuleFor(surname => surname.Surname)
                .NotNull();

            RuleFor(email => email.Email)
                .NotNull()
                .EmailAddress();

            RuleFor(phoneNumber => phoneNumber.PhoneNumber)
                .NotNull()
                .GreaterThanOrEqualTo(9);

            RuleFor(categoryId => categoryId.CategoryId)
                .InclusiveBetween(1, 3)
                .WithMessage(" 1 - Private\n 2 - Business\n 3 - Other ");
        }
    }
}

