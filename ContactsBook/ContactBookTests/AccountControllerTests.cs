using Microsoft.AspNetCore.Mvc.Testing;
using RecruitmentTask.Models;
using FluentAssertions;
using WebAppTests.Extensions;
using Xunit;
using Microsoft.EntityFrameworkCore;
using RecruitmentTask.Data;
using Microsoft.Extensions.Options;
using Moq;
using RecruitmentTask.Services;

namespace WebAppTests
{
    public class AccountControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private HttpClient _client;
        private Mock<IAccountService> _accountServiceMock = new Mock<IAccountService>();

        public AccountControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory
                .WithWebHostBuilder(builder=>
                {
                    builder.ConfigureServices(services =>
                    {
                        var dbContextOptions = services.SingleOrDefault(service => service.ServiceType == typeof(DbContextOptions<DataDbContext>));
                        services.Remove(dbContextOptions);
                        services.AddDbContext<DataDbContext>(options=>options.UseInMemoryDatabase("testDb"));

                        services.AddSingleton<IAccountService>(_accountServiceMock.Object);
                    });
                })
                .CreateClient();
        }

        [Fact]
        public async Task Register_ForValidData_ReturnOk()
        {
            var registerUser = new RegisterUser()
            {
                Name = "Krystian",
                Surname = "Sąsiadek",
                Email = "test@gmail.com",
                Password = "Password1!",
                PhoneNumber = 12345679.00,
                DateOfBirth = new DateTime(2000, 01, 26)
            };

            var httpContent = registerUser.ToJsonHttpContent();
            var response = await _client.PostAsync("/api/account/register", httpContent);
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }


        [Fact]
        public async Task Register_ForInValidData_ReturnBadRequest()
        {
            var registerUser = new RegisterUser()
            {
                Name = "Krystian",
                Surname = "Sąsiadek",
                Email = "test@test.com",
                Password = "password1",
                PhoneNumber = 12345679.0,
                DateOfBirth = new DateTime(2000, 01, 26)
            };

            var httpContent = registerUser.ToJsonHttpContent();
            var response = await _client.PostAsync("/api/account/register", httpContent);
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Login_ForRegisteredUser_ReturnStatusOk()
        {
            _accountServiceMock
                .Setup(x => x.GenerateJwt(It.IsAny<LoginUser>()))
                .Returns("jwt");

            var loginUser = new LoginUser()
            {
                Email= "test@gmail.com",
                Password= "Password1!",
            };

            var httpContent = loginUser.ToJsonHttpContent();
            var result = await _client.PostAsync("/api/account/login", httpContent);
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
    }
}
