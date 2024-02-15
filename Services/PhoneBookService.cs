using PhoneBookApp.Models;
using PhoneBookApp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBookApp.Services
{
    public class PhoneBookService : IPhoneBookService
    {

        private readonly IContactRepository _contactRepository;

        public PhoneBookService(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        public async Task<bool> CreateContactAsync(Contact contact)
        {
            return await _contactRepository.CreateAsync(contact);
        }

        public async Task<Contact?> GetContactAsync(int id)
        {
            return await _contactRepository.GetAsync(id);
        }

        public async Task<IEnumerable<Contact>> GetAllContactsAsync()
        {
            return await _contactRepository.GetAllAsync();
        }

        public async Task<bool> UpdateContactAsync(Contact newContact)
        {
            var contact = await _contactRepository.GetAsync(newContact.Id);
            if (contact == null) return false;
            contact = newContact;
            await _contactRepository.UpdateAsync(contact);
            return true;
        }


        public async Task<bool> DeleteContactAsync(int id)
        {
            var toBeDeletedContact = await _contactRepository.GetAsync(id);
            if (toBeDeletedContact == null) 
                return false;
            await _contactRepository.DeleteAsync(toBeDeletedContact);
            return true;
        }
    }
}
