using System;
using static FileCabinetApp.CommandHandlers.ValidationMethods;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Create command handler.
    /// </summary>
    public class CreateCommandHandler : CommandHandlerBase
    {
        /// <inheritdoc/>
        public override void Handle(AppCommandRequest request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (request.Command == "create")
            {
                Console.Write($"First name: ");
                string firstName = ReadInput<string>(NamesConverter, NamesValidator);

                Console.Write($"Second name: ");
                string lastName = ReadInput<string>(NamesConverter, NamesValidator);

                Console.Write("Date of birth: ");
                DateTime dateOfBirth = ReadInput<DateTime>(DateOfBirthConverter, DateOfBirthValidator);

                Console.Write("Sex (M or F): ");
                char sex = ReadInput<char>(SexConverter, SexValidator);

                Console.Write("Salary: ");
                decimal salary = ReadInput<decimal>(SalaryConverter, SalaryValidator);

                Console.Write("Years Of Service: ");
                short yearsOfService = ReadInput<short>(YearsOfServiceConverter, YearsOfServiceValidator);

                var id = Program.FileCabinetService.CreateRecord(new ParametresOfRecord(firstName, lastName, dateOfBirth, sex, salary, yearsOfService));

                Console.WriteLine($"Record #{id} is created.");
            }
            else
            {
                this.nextHandler.Handle(request);
            }
        }
    }
}