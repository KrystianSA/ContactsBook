using AutoFixture;
using ContactsBook.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;
using RecruitmentTask.Services;
using WebAppTests.Extensions;
using Xunit;

namespace ContactsBook.IntegrationTests.IntegrationTests //nie pasuje mi działania add i update. W add sprawdź czy mapowanie w ogóle działa jak należy 
{
    public class ContactsControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private HttpClient _client;
        private WebApplicationFactory<Program> _factory;
        private Mock<IContactService> _mockContactService;
        private Fixture _fixture;
        public ContactsControllerTests(WebApplicationFactory<Program> factory)
        {
            _fixture = new Fixture();
            _mockContactService = new Mock<IContactService>();
            _factory = factory
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                        services.AddScoped(serviceProvider => _mockContactService.Object);
                    });
                });
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task GetContacts_WithCorrectQuery_ReturnStatusOk()
        {
            var contacts = _fixture.CreateMany<GetContactsDto>(3);
            _mockContactService.Setup(service => service.GetAll()).Returns(contacts);
            var result = await _client.GetAsync("/contacts");
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetContactById_ForExistId_ReturnStatusOk()
        {
            var contact = _fixture.Build<GetContactsDto>()
                                 .With(id => id.Id, 1)
                                 .Create();
            _mockContactService.Setup(service => service.GetById(1)).Returns(contact);
            var result = await _client.GetAsync("/contacts/1");
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetContactById_ForNonExistId_ReturnStatusNotFound()
        {
            var contact = _fixture.Build<GetContactsDto>()
                                 .With(id => id.Id, 1)
                                 .Create();
            _mockContactService.Setup(service => service.GetById(1)).Returns(contact);
            var result = await _client.GetAsync("/contacts/3");
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeleteContact_ForExistId_ReturnNonContent()
        {
            var contact = _fixture.Build<GetContactsDto>()
                                 .With(id => id.Id, 1)
                                 .Create();
            _mockContactService.Setup(service => service.Remove(1)).Returns(true);
            var result = await _client.DeleteAsync("/contacts/1");
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task AddContact_ForValidModel_ReturnStatusOk()
        {
            var contact = _fixture.Build<CreateOrUpdateContactDto>()
                                  .With(email => email.Email, "krystian@gmail.com")
                                  .With(phoneNumber => phoneNumber.PhoneNumber, 123456789)
                                  .With(categoryId => categoryId.CategoryId, 1)
                                  .Create();
            _mockContactService.Setup(service => service.Add(contact));
            var httpContent = contact.ToJsonHttpContent();
            var result = await _client.PostAsync("/contacts", httpContent);
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task UpdateContact_ForValidModel_ReturnStatusOk()
        {
            var newContact = _fixture.Build<CreateOrUpdateContactDto>()
                                .With(email => email.Email, "krystian@gmail.com")
                                .With(phoneNumber => phoneNumber.PhoneNumber, 123456789)
                                .With(categoryId => categoryId.CategoryId, 1)
                                .Create();
            var httpContent = newContact.ToJsonHttpContent();
            var result = await _client.PutAsync("/contacts/2", httpContent);
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
    }
}
