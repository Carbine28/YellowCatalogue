using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using PhoneBookApp.Contexts;
using PhoneBookApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBookApp.Repositories
{
    public class ContactRepository : IContactRepository
    {
        private readonly PhoneBookDbContext _context;
        public ContactRepository(PhoneBookDbContext context) 
        {
            _context = context;
        }

        public async Task<bool> CreateAsync(Contact contact)
        {
            await _context.AddAsync(contact);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Contact?> GetAsync(int id)
        {
            return await _context.Contacts.FindAsync(id);
        }

        public async Task<IEnumerable<Contact>> GetAllAsync()
        {
            return await _context.Contacts.ToListAsync();
        }

        public async Task UpdateAsync(Contact contact)
        {
            _context.Contacts.Update(contact);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Contact contact)
        {
            _context.Remove(contact);
            await _context.SaveChangesAsync();
        }
       
    }
}
