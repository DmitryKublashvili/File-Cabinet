using System;
using static FileCabinetApp.CommandHandlers.ValidationMethods;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Edit command handler.
    /// </summary>
    public class EditCommandHandler : CommandHandlerBase
    {
        /// <inheritdoc/>
        public override void Handle(AppCommandRequest request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (request.Command == "edit")
            {
                if (!int.TryParse(request.Parameters, out int id))
                {
                    Console.WriteLine("Incorrect input.");
                    return;
                }

                if (!Program.FileCabinetService.IsRecordExist(id))
                {
                    Console.WriteLine($"#{request.Parameters} record is not found.");
                    return;
                }

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

                Program.FileCabinetService.EditRecord(new ParametresOfRecord(id, firstName, lastName, dateOfBirth, sex, salary, yearsOfService));

                Console.WriteLine($"Record #{id} is updated.");
            }
            else
            {
                this.nextHandler.Handle(request);
            }
        }
    }
}