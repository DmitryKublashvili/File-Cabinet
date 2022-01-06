using System;
using System.Collections.Generic;
using System.Globalization;
using FileCabinetApp.Iterators;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Find command handler.
    /// </summary>
    public class FindCommandHandler : ServiceCommandHandlerBase
    {
        private const string NoMatchesMessage = "No matches were found.";

        private readonly Action<IRecordIterator> printer;

        /// <summary>
        /// Initializes a new instance of the <see cref="FindCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Some instance implemented IFileCabinetService.</param>
        /// <param name="printer">Concrete printer.</param>
        public FindCommandHandler(IFileCabinetService service, Action<IRecordIterator> printer)
            : base(service)
        {
            this.printer = printer;
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

                IRecordIterator iterator;

                if (firstParameter == "FIRSTNAME")
                {
                    iterator = this.service.FindByFirstName(secondParameter);
                }
                else if (firstParameter == "LASTNAME")
                {
                    iterator = this.service.FindByLastName(secondParameter);
                }
                else
                {
                    if (DateTime.TryParse(secondParameter, Program.CultureInfoSettings, DateTimeStyles.AdjustToUniversal, out DateTime date))
                    {
                        iterator = this.service.FindByDateOfBirth(date);
                    }
                    else
                    {
                        Console.WriteLine($"Date of birth '{secondParameter}' was incorrect format.");
                        return;
                    }
                }

                if (!iterator.HasMore())
                {
                    Console.WriteLine(NoMatchesMessage);
                    return;
                }

                this.printer?.Invoke(iterator);
            }
            else
            {
                this.nextHandler.Handle(request);
            }
        }
    }
}