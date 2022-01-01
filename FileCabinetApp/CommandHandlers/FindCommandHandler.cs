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

        private readonly CultureInfo cultureInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="FindCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Some instance implemented IFileCabinetService.</param>
        /// <param name="cultureInfo">CultureInfo settings.</param>
        public FindCommandHandler(IFileCabinetService service, CultureInfo cultureInfo)
            : base(service)
        {
            this.cultureInfo = cultureInfo;
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
                    if (DateTime.TryParse(secondParameter, this.cultureInfo, DateTimeStyles.AdjustToUniversal, out DateTime date))
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
                        $"{foundRecords[i].DateOfBirth.ToString("yyyy-MMM-d", this.cultureInfo)}, " +
                        $"Sex - {foundRecords[i].Sex}, Salary {foundRecords[i].Salary.ToString(this.cultureInfo)}, " +
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