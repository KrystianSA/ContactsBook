using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using RecruitmentTask.Data;
using RecruitmentTask.Entities;
using RecruitmentTask.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RecruitmentTask.Services
{
    public class AccountService : IAccountService
    {
        private readonly DataDbContext _dbContext;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly AutheticationSettings _autheticationSettings;

        public AccountService(DataDbContext dbContext, IPasswordHasher<User> passwordHasher, AutheticationSettings autheticationSettings)
        {
            _dbContext = dbContext;
            _passwordHasher = passwordHasher;
            _autheticationSettings = autheticationSettings;
        }

        public void Register(RegisterUser user)
        {
            var newUser = new User()
            {
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,
                Password = user.Password,
                PhoneNumber = user.PhoneNumber,
                DateOfBirth = user.DateOfBirth
            };

            var hashPasssword = _passwordHasher.HashPassword(newUser, user.Password);
            newUser.Password = hashPasssword;

            _dbContext.Users.Add(newUser);
            _dbContext.SaveChanges();
        }

        public string GenerateJwt(LoginUser login)
        {
            var user = _dbContext.Users.FirstOrDefault(user => user.Email == login.Email);

            var result = _passwordHasher.VerifyHashedPassword(user, user.Password, login.Password);

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.Name} {user.Surname}")
            };

            var keyJwt = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_autheticationSettings.JwtKey));
            var cred = new SigningCredentials(keyJwt, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(_autheticationSettings.JwtExpireDays);

            var token = new JwtSecurityToken(
                _autheticationSettings.JwtIssuer,
                _autheticationSettings.JwtIssuer,
                claims,
                expires: expires,
                signingCredentials: cred);

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }
    }
}
