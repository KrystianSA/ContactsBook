using FluentValidation;
using RecruitmentTask.Data;
using RecruitmentTask.Entities;
using RecruitmentTask.Models;

namespace WebApp.Models.Validators
{
    public class RegisterUserValidator : AbstractValidator<RegisterUser>
    {
        public RegisterUserValidator(DataDbContext dbContext)
        {
            RuleFor(email => email.Email)
                .NotNull()
                .EmailAddress();

            RuleFor(password => password.Password)
                .MinimumLength(8).WithMessage("Minimum length 6 letters")
                .Matches(@"[A-Z]+").WithMessage("Password must contain minimum one strong letter")
                .Matches(@"[\!\?\*\.]").WithMessage("Password must contain minimum one special sign");

            RuleFor(email => email.Email)
               .Custom((value, context) =>
               {
                   var emailInUse = dbContext.Users.Any(u => u.Email == value);
                   if (emailInUse)
                   {
                       context.AddFailure("Email", "That email is taken");
                   }
               });

            //Dopisać Validacje dla numeru telefonu
        }
    }
}
