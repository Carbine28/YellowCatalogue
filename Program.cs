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
            //// Ask for the user's favorite fruits
            //var fruits = AnsiConsole.Prompt(
            //    new MultiSelectionPrompt<string>()
            //        .Title("What are your [green]favorite fruits[/]?")
            //        .NotRequired() // Not required to have a favorite fruit
            //        .PageSize(10)
            //        .MoreChoicesText("[grey](Move up and down to reveal more fruits)[/]")
            //        .InstructionsText(
            //            "[grey](Press [blue]<space>[/] to toggle a fruit, " +
            //            "[green]<enter>[/] to accept)[/]")
            //        .AddChoices(new[] {
            //"Apple", "Apricot", "Avocado",
            //"Banana", "Blackcurrant", "Blueberry",
            //"Cherry", "Cloudberry", "Coconut",
            //        }));

            //// Write the selected fruits to the terminal
            //foreach (string fruit in fruits)
            //{
            //    AnsiConsole.WriteLine(fruit);
            //}


            ////var newContact = new Contact { Name = "Mike Ross", Email = "random@gmail.com", PhoneNumber = "9090320420" };
            //while (true)
            //{
            //    // Display Welcome Message
            //    // Display Menu Controls 

            //    // Handle Menu switch

            //}
            //AnsiConsole.Markup("[underline red]Hello[/] World!");
            // Ask for the user's favorite fruit
            // Ask for the user's favorite fruits


            // Write centered cell grid contents to Console
            //AnsiConsole.Write(grid);


        }


    }
}
