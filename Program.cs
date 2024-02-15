using PhoneBookApp.Contexts;
using PhoneBookApp.Repositories;
using PhoneBookApp.Services;
using Spectre.Console;
using Spectre.Cli;
namespace PhoneBookApp
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            bool debug = false;
            if(!debug)
            {
                using var dbContext = new PhoneBookDbContext();
                var contactRepository = new ContactRepository(dbContext);
                var phoneBookService = new PhoneBookService(contactRepository);

                YellowCatalogue app = new YellowCatalogue(phoneBookService);
                await app.Start();
            }
        }
    }
}
