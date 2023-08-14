using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecruitmentTask.Data;
using RecruitmentTask.Entities;
using RecruitmentTask.Models;
using RecruitmentTask.Services;
using System.Collections.Generic;

namespace RecruitmentTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowOrigin")]
    [Authorize]
    public class ContactsController : ControllerBase
    {
        private readonly IContactService _contactsService;
        private readonly DataDbContext _dbContext;

        public ContactsController(IContactService contactsService, DataDbContext dbContext)
        {
            _contactsService = contactsService;
            _dbContext = dbContext;
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult<IEnumerable<Contact>> GetContacts()
        {
            var contacts = _contactsService.GetAll();
            return Ok(contacts);
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult GetContactById([FromRoute] int id)
        {
            var contact = _contactsService.GetById(id);
            if(contact == null) 
            { 
                return NotFound();
            }
            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public ActionResult DeleteContact([FromRoute] int id)
        {
            var isDeleted = _contactsService.Remove(id);

            if (isDeleted)
            {
                return NoContent();
            }

            return NotFound();
        }

        [HttpPost]
        public ActionResult AddContact([FromBody] Contact contact) 
        {
            try
            {
                var newUser = _contactsService.Add(contact);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("{id}")]
        public ActionResult UpdateContact([FromBody] Contact user, [FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var isUpdated = _contactsService.Update(user, id);
            return Ok();
        }
    }
}