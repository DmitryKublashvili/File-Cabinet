using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Find command handler.
    /// </summary>
    public class FindCommandHandler : ServiceCommandHandlerBase
    {
        private const string NoMatchesMessage = "No matches were found.";

        /// <summary>
        /// Initializes a new instance of the <see cref="FindCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Some instance implemented IFileCabinetService.</param>
        public FindCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        /// <inheritdoc/>
        public override void Handle(AppCommandRequest request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (request.Command == "find")
            {
                string[] userCommandParameters = request.Parameters.Split(new char[] { ' ', '"' }, 2, StringSplitOptions.RemoveEmptyEntries);

                List<string> commandParametres = new List<string>()
                {
                "FIRSTNAME",
                "LASTNAME",
                "DATEOFBIRTH",
                };

                string firstParameter = userCommandParameters[0].ToUpperInvariant();

                if (!commandParametres.Contains(firstParameter))
                {
                    Console.WriteLine($"Unknown parameter '{userCommandParameters[0]}'.");
                    return;
                }

                string secondParameter = userCommandParameters[1].Trim('"', ' ').ToUpperInvariant();
                ReadOnlyCollection<FileCabinetRecord> foundRecords;

                if (firstParameter == "FIRSTNAME")
                {
                    foundRecords = this.service.FindByFirstName(secondParameter);
                }
                else if (firstParameter == "LASTNAME")
                {
                    foundRecords = this.service.FindByLastName(secondParameter);
                }
                else
                {
                    if (DateTime.TryParse(secondParameter, Program.CultureInfoSettings, DateTimeStyles.AdjustToUniversal, out DateTime date))
                    {
                        foundRecords = this.service.FindByDateOfBirth(date);
                    }
                    else
                    {
                        Console.WriteLine($"Date of birth '{secondParameter}' was incorrect format.");
                        return;
                    }
                }

                if (foundRecords.Count == 0)
                {
                    Console.WriteLine(NoMatchesMessage);
                    return;
                }

                for (int i = 0; i < foundRecords.Count; i++)
                {
                    Console.WriteLine(
                        $"#{foundRecords[i].Id}, {foundRecords[i].FirstName}, {foundRecords[i].LastName}, " +
                        $"{foundRecords[i].DateOfBirth.ToString("yyyy-MMM-d", Program.CultureInfoSettings)}, " +
                        $"Sex - {foundRecords[i].Sex}, Salary {foundRecords[i].Salary.ToString(Program.CultureInfoSettings)}, " +
                        $"{foundRecords[i].YearsOfService} years Of Service, ");
                }
            }
            else
            {
                this.nextHandler.Handle(request);
            }
        }
    }
}