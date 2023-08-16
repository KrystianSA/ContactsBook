using AutoMapper;
using ContactsBook.Models;
using RecruitmentTask.Entities;

namespace RecruitmentTask.Mapper
{
    public class ContactsMappingProfile : Profile
    {
        public ContactsMappingProfile()
        {
            CreateMap<Contact, GetContactsDto>()
                .ForMember(dst=> dst.CategoryName,opt=>opt.MapFrom(src=>src.Category.Name));

            CreateMap<Contact, CreateContactDto>();
        }
    }
}
