using FluentValidation;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ContactsBook.Models.Validators
{
    public class UpdateContactDtoValidator : ValidationBaseForUpdateAndCreateContact<CreateOrUpdateContactDto>
    {
        public UpdateContactDtoValidator()
        {
        }
    }
}
