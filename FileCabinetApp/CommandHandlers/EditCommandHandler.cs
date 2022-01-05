using System;
using static FileCabinetApp.CommandHandlers.ValidationMethods;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Edit command handler.
    /// </summary>
    public class EditCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EditCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Some instance implemented IFileCabinetService.</param>
        public EditCommandHandler(IFileCabinetService service)
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

            if (request.Command == "edit")
            {
                if (!int.TryParse(request.Parameters, out int id))
                {
                    Console.WriteLine("Incorrect input.");
                    return;
                }

                if (!this.service.IsRecordExist(id))
                {
                    Console.WriteLine($"#{request.Parameters} record is not found.");
                    return;
                }

                Console.Write($"First name: ");
                string firstName = ReadInput<string>(NamesConverter, NamesValidator);

                Console.Write($"Second name: ");
                string lastName = ReadInput<string>(NamesConverter, NamesValidator);

                Console.Write("Date of birth: ");
                DateTime dateOfBirth = ReadInput(DateOfBirthConverter, ValidationMethods.DateOfBirthValidator);

                Console.Write("Sex (M or F): ");
                char sex = ReadInput(SexConverter, ValidationMethods.SexValidator);

                Console.Write("Salary: ");
                decimal salary = ReadInput(SalaryConverter, ValidationMethods.SalaryValidator);

                Console.Write("Years Of Service: ");
                short yearsOfService = ReadInput(YearsOfServiceConverter, ValidationMethods.YearsOfServiceValidator);

                var result = this.service.EditRecord(new ParametresOfRecord(id, firstName, lastName, dateOfBirth, sex, salary, yearsOfService));

                if (result)
                {
                    Console.WriteLine($"Record #{id} is updated.");
                }
                else
                {
                    Console.WriteLine($"Record #{id} is not found.");
                }
            }
            else
            {
                this.nextHandler.Handle(request);
            }
        }
    }
}