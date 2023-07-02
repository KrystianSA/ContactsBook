using FluentAssertions;
using FluentAssertions.Execution;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Moq;
using RecruitmentTask.Data;
using RecruitmentTask.Entities;
using RecruitmentTask.Models;
using RecruitmentTask.Services;
using System.ComponentModel.DataAnnotations;
using WebAppTests.Extensions;
using Xunit;
using Xunit.Sdk;

namespace ContactsBook.IntegrationTests
{
    public class ContactsControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private HttpClient _client;
        private WebApplicationFactory<Program> _factory;

        public ContactsControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        var dbContextOptions = services.SingleOrDefault(service => service.ServiceType == typeof(DbContextOptions<DataDbContext>));
                        services.Remove(dbContextOptions);
                        services.AddDbContext<DataDbContext>(options => options.UseInMemoryDatabase("testDb"));

                        services.AddSingleton<IPolicyEvaluator, FakePolicyEvaluator>();
                    });
                });

            _client=_factory.CreateClient();

        }

        private void Seeder(Contact contact)
        {
            var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();

            using var scope = scopeFactory.CreateScope();

            var _dbContext = scope.ServiceProvider.GetService<DataDbContext>();

            if (_dbContext.Contacts.SingleOrDefault(x => x.Id == contact.Id) == null)
            {
                _dbContext.Contacts.Add(contact);
                _dbContext.SaveChanges();
            }
        }

        [Fact]
        public async Task GetContacts_WithCorrectQuery_ReturnStatusOk()
        {
            var result = await _client.GetAsync("/api/contacts");
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetContactById_ForExistId_ReturnStatusOk()
        {
            var contact = new Contact()
            {
                Id = 2,
                Name = "Krystian",
                Surname = "Sąsiadek",
                Email = "test@test.com",
                PhoneNumber = 1232323,
                CategoryId = 1
            };
            var result = await _client.GetAsync("/api/contacts/"+contact.Id);
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task DeleteContact_ForExistId_ReturnNonContent()
        {
            var contact = new Contact()
            {
                Id = 2,
                Name = "Krystian",
                Surname = "Sąsiadek",
                Email = "test@test.com",
                PhoneNumber = 1232323,
                CategoryId = 1
            };
            var result = await _client.DeleteAsync("/api/contacts/" + contact.Id);
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
            var result = await _client.PostAsync("/api/contacts/", httpContent);
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
    }
}
