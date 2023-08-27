using AutoFixture;
using AutoMapper;
using ContactsBook.Models;
using RecruitmentTask.Entities;
using FluentAssertions;
using Xunit;
using RecruitmentTask.Mapper;

namespace ContactsBook.IntegrationTests
{
    public class ContactServiceTests
    {
        private readonly IMapper _mapper;
        private readonly Fixture _fixture;

        public ContactServiceTests()
        {
            _fixture = new Fixture();

            var configuration = new MapperConfiguration(cfg =>
            { 
                cfg.AddProfile<ContactsMappingProfile>();
            });
            _mapper = configuration.CreateMapper();
        }

        [Fact]
        public void Add_ForEntryModel_ReturnMapperModel()
        {
            var contact = _fixture.Create<CreateOrUpdateContactDto>();
            var contactDto = _mapper.Map<Contact>(contact);
            contactDto.Should().NotBeNull();
        }
    }
}
