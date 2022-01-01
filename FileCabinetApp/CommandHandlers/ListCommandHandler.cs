using System;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// List command handler.
    /// </summary>
    public class ListCommandHandler : ServiceCommandHandlerBase
    {
        private const string ThereAreNoRecordsMessage = "There are no records yet.";

        /// <summary>
        /// Initializes a new instance of the <see cref="ListCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Some instance implemented IFileCabinetService.</param>
        public ListCommandHandler(IFileCabinetService service)
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

            if (request.Command == "list")
            {
                var list = this.service.GetRecords();

                if (list is null || list.Count == 0)
                {
                    Console.WriteLine(ThereAreNoRecordsMessage);
                    return;
                }

                for (int i = 0; i < list.Count; i++)
                {
                    Console.WriteLine(
                        $"#{list[i].Id}, {list[i].FirstName}, {list[i].LastName}, " +
                        $"{list[i].DateOfBirth.ToString("yyyy-MMM-d", Program.CultureInfoSettings)}, " +
                        $"Sex - {list[i].Sex}, Salary {list[i].Salary.ToString(Program.CultureInfoSettings)}, " +
                        $"{list[i].YearsOfService} years Of Service");
                }
            }
            else
            {
                this.nextHandler.Handle(request);
            }
        }
    }
}