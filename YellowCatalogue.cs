using PhoneBookApp.Services;
using PhoneBookApp.Models;
using Spectre.Console;
using System.Globalization;
using System.Collections;
using System.Reflection.Metadata;
using System;
using System.Data;

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
            while (SelectedState != State.TerminateProgram)
            {
                Console.WriteLine();
                ResetState();
                await HandleState(SelectedState);

            }

            return true;
        }
        private State DisplayAndSelectMenuChoices()
        {
            //DisplayRule("[white]Select an option[/]", "red dim");
            var newRule = new Spectre.Console.Rule("[white]Select an option[/]");
            newRule.LeftJustified();
            newRule.RuleStyle("red dim");
            AnsiConsole.Write(newRule);
            // Display Selection Message
            var option = AnsiConsole.Prompt(
                new SelectionPrompt<String>()
                    .Title("")
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
                    AnsiConsole.MarkupLine("Welcome to the [underline yellow]Yellow Catalogue![/]");
                    SelectedState = DisplayAndSelectMenuChoices();
                    await HandleState(SelectedState);
                    break;
                case State.Read:
                    await ReadAll();
                    break;
                case State.Create:
                    await HandleCreate();
                    break;
                case State.Update:
                    await HandleUpdate();
                    break;
                case State.Delete:
                    await ReadAll();
                    await HandleDelete();
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

        private async Task HandleDelete()
        {
            var Id = "";
            int parsedId;
            Id = AnsiConsole.Ask<string>("Enter an [underline blue]ID[/] to be [underline bold red]deleted[/]");
            while (!Int32.TryParse(Id, out parsedId))
            {
                AnsiConsole.MarkupLine("[yellow underline]Incorrect ID[/] entered, try again");
                Id = AnsiConsole.Ask<string>("Enter an [underline blue]ID[/] to be [underline bold red]deleted[/]");
            }

            if(!AnsiConsole.Confirm($"Are you sure you want to delete contact with ID: {parsedId}?"))
            {
                Console.WriteLine("Phew... :]");
                return;
            }
            AnsiConsole.MarkupLine("Ok... :(");
            var deleted =  await _phoneBookService.DeleteContactAsync(parsedId);
            if (deleted)
                AnsiConsole.MarkupLine("Deletion [green]successful[/]!");
            else
                AnsiConsole.MarkupLine("Deletion [red underline]failed[/]. Contact not found");
        }
        
        private async Task HandleCreate()
        {
            //DisplayRule("[white] Contact Creation[/]", "red dim");
            var newRule = new Spectre.Console.Rule("[white]Contact Creation[/]");
            newRule.LeftJustified();
            newRule.RuleStyle("yellow dim");
            AnsiConsole.Write(newRule);

            await CreateContact(null);
        }

        private async Task HandleUpdate()
        {
            Console.Clear();
            await ReadAll();

            var newRule = new Spectre.Console.Rule("[white]ID Selection[/]");
            newRule.LeftJustified();
            newRule.RuleStyle("yellow dim");
            AnsiConsole.Write(newRule);

            int parsedId;
            var Id = AnsiConsole.Ask<string>("Enter an [underline blue]ID[/] to be [underline bold yellow]updated[/]");
            while (!Int32.TryParse(Id, out parsedId))
            {
                AnsiConsole.MarkupLine("[yellow underline]Incorrect ID[/] entered, try again");
                Id = AnsiConsole.Ask<string>("Enter an [underline blue]ID[/] to be [underline bold red]deleted[/]");
            }

            Contact contact = await _phoneBookService.GetContactAsync(parsedId);
            if (contact == null)
            {
                Console.WriteLine("Contact not found in yellow catalogue.");
                return;
            }

            // Call function to handle contact, allow updates etc
            await CreateContact(contact);
        }

        private async Task CreateContact(Contact? contact)
        {

            Contact newContact;
            if (contact == null) newContact = new Contact();
            else newContact = contact;
            DisplayContact(newContact);
            Dictionary<string, bool> confirmedAnswers = new Dictionary<string, bool>();
            while (true)
            {

                var prompt = new SelectionPrompt<String>()
                        .Title("Select a property to modify")
                        .PageSize(10)
                        .AddChoiceGroup("Properties", [
                            "Name",
                            "Email",
                            "Phone Number"])
                        .AddChoices(new[] { "Cancel" });

                if (confirmedAnswers.Count() >= 3)
                {
                    prompt.AddChoice("Submit");
                }

                var option = AnsiConsole.Prompt(prompt);


                if (option == "Cancel")
                {
                    if (ConfirmUser("Are you sure you want to [red underline]quit?[/]"))
                    {
                        break;
                    }
                    continue;
                }

                if (option == "Submit")
                {
                    if (ConfirmUser("[green underline]Finish[/] creating the contact?"))
                    {
                        if (contact == null)
                            await CreateNewContact(newContact);
                        else
                            await UpdateContact(contact);
                        break;
                    }
                    continue;
                }

                var ans = PromptUser($"Enter in your {option}: ");
                // TODO: Handle verification here
                if (option == "Name")
                    newContact.Name = ans;
                if (option == "Email")
                    newContact.Email = ans;
                if (option == "Phone Number")
                    newContact.PhoneNumber = ans;
                //confirmedAnswers.Add(option, true);
                confirmedAnswers[option] = true;
                Console.Clear();
                var rule = new Spectre.Console.Rule("[white]Contact Creation[/]");
                rule.LeftJustified();
                rule.RuleStyle("yellow dim");
                AnsiConsole.Write(rule);
                DisplayContact(newContact);
            }
        }
        private void ResetState() => SelectedState = State.Menu;
        
        private void DisplayContact(Contact? contact = null)
        {
            // Draw Basic grid
            var grid = new Grid();
            grid.AddColumn();
            grid.AddColumn();
            grid.AddColumn();
            grid.AddColumn();

            grid.AddRow(new Text[]{
                //new Text("ID", new Style(Color.Red, Color.Black)).LeftJustified(),
                new Text("Name", new Style(Color.Blue, Color.Black)).LeftJustified(),
                new Text("Email", new Style(Color.Green, Color.Black)).Centered(),
                new Text("Phone Number", new Style(Color.Orange1, Color.Black)).RightJustified(),
            });

            if (contact != null)
            {
                grid.AddRow(new Text[]
                {
                    //new Text(contact.Id.ToString()).LeftJustified(),
                    new Text(contact.Name + "").LeftJustified(),
                    new Text(contact.Email + "").Centered(),
                    new Text(contact.PhoneNumber + "").RightJustified(),
                });
            }

            AnsiConsole.Write(grid);
            Console.WriteLine();
        }

        private static string PromptUser(string prompt)
        {
            var answer = AnsiConsole.Ask<string>(prompt);
            return answer;
        }

        private static bool ConfirmUser(string prompt)
        {
            if(!AnsiConsole.Confirm(prompt))
            {
                return false;
            }
            return true;
        }
        private static void CleanUpProgram()
        {
            AnsiConsole.MarkupLine("Thanks for visiting the [underline yellow]Yellow Catalogue![/]. Until next time!");
        }
        private static void DisplayRule(string rule = "", string style = "yellow dim")
        {
            var newRule = new Spectre.Console.Rule(rule);
            newRule.LeftJustified();
            newRule.RuleStyle("yellow dim");
            AnsiConsole.Write(rule);
        }

        private async Task CreateNewContact(Contact contact)
        {
            await _phoneBookService.CreateContactAsync(contact);
            AnsiConsole.MarkupLine("[green]Successfully Submitted![/]");
        }

        private async Task UpdateContact(Contact contact)
        {
            bool res = await _phoneBookService.UpdateContactAsync(contact);
            if (res)
                AnsiConsole.MarkupLine("[green]Successfully Submitted![/]");
            else
                AnsiConsole.MarkupLine("[red]Error occured[/] during update submission");
        }

    }
}
