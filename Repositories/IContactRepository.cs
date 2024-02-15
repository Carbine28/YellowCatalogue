using PhoneBookApp.Models;

namespace PhoneBookApp.Repositories
{
    public interface IContactRepository
    {
        Task<bool> CreateAsync(Contact contact);

        Task<Contact?> GetAsync(int id);

        Task<IEnumerable<Contact>> GetAllAsync();

        Task UpdateAsync(Contact contact);

        Task DeleteAsync(Contact contact);

    }
}
