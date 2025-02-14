using Shoper.Application.Dtos.ContactDtos;
using Shoper.Application.Dtos.HelpDtos;
using Shoper.Application.Interfaces;
using Shoper.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Shoper.Application.Usecasess.ContactServices
{
    public class ContactService : IContactService
    {
        private readonly IRepository<Contact> _repository;

        public ContactService(IRepository<Contact> repository)
        {
            _repository = repository;
        }

        public async Task CreateContactAsync(CreateContactDto model)
        {
            await _repository.CreateAsync(new Contact
            {
                Name = model.Name,
                Email = model.Email,
                Message = model.Message,
                CreatedDate = model.CreatedDate,
            });
        }

        public async Task DeleteContactAsync(int id)
        {
            var contact = await _repository.GetByIdAsync(id);
            await _repository.DeleteAsync(contact);
        }

        public async Task<List<ResultContactDto>> GetAllContactAsync()
        {
            var contacts = await _repository.GetAllAsync();
            return contacts.Select(x => new ResultContactDto
            {
                Id = x.Id,
                Name = x.Name,
                Email = x.Email,
                Message = x.Message,
                CreatedDate = x.CreatedDate,
            }).ToList();
        }

        public async Task<List<ResultContactDto>> GetAllContactsByEmailAsync(string email)
        {
            Expression<Func<Contact, bool>> filter = x => x.Email == email;
            var contact = await _repository.WhereAsync(filter);
            return contact.Select(x => new ResultContactDto
            {
                Id = x.Id,
                Name = x.Name,
                Email = x.Email,
                Message = x.Message,
                CreatedDate = x.CreatedDate,
            }).ToList();
        }

        public async Task<GetByIdContactDto> GetByIdContactAsync(int id)
        {
            var contact = await _repository.GetByIdAsync(id);
            var newcontact = new GetByIdContactDto
            {
                Id = contact.Id,
                Name = contact.Name,
                Email = contact.Email,
                Message = contact.Message,
                CreatedDate = contact.CreatedDate,
            };
            return newcontact;
        }

        public async Task UpdateContactAsync(UpdateContactDto model)
        {
            var contact = await _repository.GetByIdAsync(model.Id);
            contact.Name = model.Name;
            contact.Email = model.Email;
            contact.Message = model.Message;
            contact.CreatedDate = model.CreatedDate;
            await _repository.UpdateAsync(contact);
        }
    }
}
