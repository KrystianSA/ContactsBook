using AutoFixture;
using ContactsBook.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Moq;
using RecruitmentTask.Controllers;
using RecruitmentTask.Data;
using RecruitmentTask.Entities;
using RecruitmentTask.Services;
using System.ComponentModel;
using WebAppTests.Extensions;
using Xunit;

namespace ContactsBook.IntegrationTests
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
                        services.AddScoped<IContactService>(serviceProvider => _mockContactService.Object);
                    });
                });
            _client = _factory.CreateClient();
        }

        //Spróbuj użyć MOQ to testów, szczególnie dla wyszukiwania elementów których nie ma

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
            var result = await _client.GetAsync("/contacts/1");
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetContactById_ForNonExistId_ReturnStatusNotFound()
        {
            var result = await _client.GetAsync($"/contacts/");
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task DeleteContact_ForExistId_ReturnNonContent()
        {
            var result = await _client.DeleteAsync("/contacts/2");
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task AddContact_ForValidModel_ReturnStatusOk()
        {
            var contact = new Contact()
            {
                Name = "Krystian",
                Surname = "Sąsiadek",
                Email = "test1@test.com",
                PhoneNumber = 1232323,
                CategoryId = 1
            };
            var httpContent = contact.ToJsonHttpContent();
            var result = await _client.PostAsync("/contacts/", httpContent);
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task UpdateContact_ForValidModel_ReturnStatusOk()
        {
            var contact = new Contact()
            {
                Id = 2,
                Name = "Krystian",
                Surname = "Sąsiadek",
                Email = "test1@test.com",
                PhoneNumber = 1232323,
                CategoryId = 1
            };

            contact.Name = "Paweł";
            var httpContent = contact.ToJsonHttpContent();
            var result = await _client.PutAsync("/contacts/2", httpContent);
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
    }
}
