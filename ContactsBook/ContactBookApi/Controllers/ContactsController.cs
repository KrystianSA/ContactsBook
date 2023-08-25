using AutoMapper;
using ContactsBook.Models;
using FluentValidation;
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
    [Route("[controller]")]
    [ApiController]
    //[Authorize]
    public class ContactsController : ControllerBase
    {
        private readonly IContactService _contactsService;

        public ContactsController(IContactService contactsService)
        {
            _contactsService = contactsService;
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
            if (contact == null)
            {
                return NotFound();
            }
            return Ok(contact);
        }

        [HttpDelete]
        [Route("{id}")]
        public ActionResult DeleteContact([FromRoute] int id)
        {
            var isDeleted = _contactsService.Remove(id);

            if (!isDeleted)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost]
        public ActionResult AddContact([FromBody] CreateOrUpdateContactDto createContactDto)
        {
            try
            {
                _contactsService.Add(createContactDto);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("{id}")]
        public ActionResult UpdateContact([FromBody] CreateOrUpdateContactDto updateContactDto, [FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var isUpdated = _contactsService.Update(updateContactDto, id);
            return Ok();
        }
    }
}