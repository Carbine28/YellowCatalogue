using PhoneBookApp.Services;
using PhoneBookApp.Models;
using Spectre.Console;

namespace PhoneBookApp
{

    enum State
    {
        Menu,
        Read,
        Create,
        Update,
        Delete,
        TerminateProgram
    }

    public class YellowCatalogue
    {
        private IPhoneBookService _phoneBookService;
        private State SelectedState { get; set; } 

        public YellowCatalogue(IPhoneBookService phoneBookService) 
        { 
            _phoneBookService = phoneBookService;
            SelectedState = State.Menu;
        }

        public async Task<bool> Start()
        {
            // Display Welcome Message
            AnsiConsole.MarkupLine("Welcome to the [underline yellow]Yellow Catalogue![/]");
            while (SelectedState != State.TerminateProgram)
            {
                ResetState();
                await HandleState(SelectedState);

            }

            return true;
        }
        private State DisplayAndSelectMenuChoices()
        {
            // Display Selection Message
            var option = AnsiConsole.Prompt(
                new SelectionPrompt<String>()
                    .Title("Select an Option")
                    .PageSize(6)
                    .AddChoices([
                        "Read", "Create", "Update", "Delete",
                        "TerminateProgram"
                    ]));
            // Assign State
            Enum.TryParse(option, out State state); 
            return state;
        }

        private async Task HandleState(State state)
        {
            switch(state)
            {
                case State.Menu:
                    SelectedState = DisplayAndSelectMenuChoices();
                    await HandleState(SelectedState);
                    break;
                case State.Read:
                    await ReadAll();
                    break;
                case State.Create:

                    break;
                case State.Update:
                    break;
                case State.Delete:
                    await ReadAll();
                    await PromptDelete();
                    break;
                case State.TerminateProgram:
                    CleanUpProgram();
                    break;
                default:
                    Console.WriteLine("Error in reading and displaying state");
                    break;
            }
        }

        private async Task ReadAll()
        {
            List<Contact> contactList =  (await _phoneBookService.GetAllContactsAsync()).ToList();

            var grid = new Grid();
            grid.AddColumn();
            grid.AddColumn();
            grid.AddColumn();
            grid.AddColumn();

            grid.AddRow(new Text[]{
                new Text("ID", new Style(Color.Red, Color.Black)).LeftJustified(),
                new Text("Name", new Style(Color.Blue, Color.Black)).LeftJustified(),
                new Text("Email", new Style(Color.Green, Color.Black)).Centered(),
                new Text("Phone Number", new Style(Color.Orange1, Color.Black)).RightJustified(),
            });

            foreach (Contact contact in contactList)
            {
                grid.AddRow(new Text[]
                {
                    new Text(contact.Id.ToString()).LeftJustified(),
                    new Text(contact.Name + "").LeftJustified(),
                    new Text(contact.Email + "").Centered(),
                    new Text(contact.PhoneNumber + "").RightJustified(),
                });
            }
            AnsiConsole.Write(grid);
            Console.WriteLine();
        }

        private async Task PromptDelete()
        {
            var Id = "";
            int parsedId;
            while(!Int32.TryParse(Id, out parsedId))
            {
                Id = AnsiConsole.Ask<string>("Enter an [underline blue]ID[/] to be [underline bold red]deleted[/]");
            }

            var deleted =  await _phoneBookService.DeleteContactAsync(parsedId);
            if (deleted)
                AnsiConsole.MarkupLine("Deletion [green]successful[/]!");
            else
                AnsiConsole.MarkupLine("Deletion [red underline]failed[/]. Contact not found");
        }

        private void CleanUpProgram()
        {
            AnsiConsole.MarkupLine("Thanks for visiting the [underline yellow]Yellow Catalogue![/]. Until next time!");
        }
        private void ResetState() => SelectedState = State.Menu;
        
    }
}
