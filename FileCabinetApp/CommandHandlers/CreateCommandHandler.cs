using System;
using static FileCabinetApp.CommandHandlers.ValidationMethods;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Create command handler.
    /// </summary>
    public class CreateCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Some instance implemented IFileCabinetService.</param>
        public CreateCommandHandler(IFileCabinetService service)
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

            if (request.Command == "create")
            {
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

                var id = this.service.CreateRecord(new ParametresOfRecord(firstName, lastName, dateOfBirth, sex, salary, yearsOfService));

                Console.WriteLine($"Record #{id} is created.");
            }
            else
            {
                this.nextHandler.Handle(request);
            }
        }
    }
}