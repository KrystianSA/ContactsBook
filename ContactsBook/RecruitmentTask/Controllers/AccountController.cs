using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RecruitmentTask.Data;
using RecruitmentTask.Entities;
using RecruitmentTask.Models;
using RecruitmentTask.Services;

namespace RecruitmentTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("register")]
        public ActionResult Register([FromBody] RegisterUser user)
        {
            _accountService.Register(user);
            return Ok();
        }

        [HttpPost("login")]
        public ActionResult Login([FromBody] LoginUser login)
        {
            string token = _accountService.GenerateJwt(login);
            
            return Ok(token);
        }

    }
}
