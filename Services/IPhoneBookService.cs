using PhoneBookApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBookApp.Services
{
    public interface IPhoneBookService
    {
        Task<bool> CreateContactAsync(Contact contact);

        Task<Contact> GetContactAsync(int id);

        Task<IEnumerable<Contact>> GetAllContactsAsync();

        Task<bool> UpdateContactAsync(Contact newContact);

        Task<bool> DeleteContactAsync(int id);
    }
}
