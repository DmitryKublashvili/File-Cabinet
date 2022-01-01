using System;
using FileCabinetApp.Printers;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// List command handler.
    /// </summary>
    public class ListCommandHandler : ServiceCommandHandlerBase
    {
        private const string ThereAreNoRecordsMessage = "There are no records yet.";

        private readonly IRecordPrinter printer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Some instance implemented IFileCabinetService.</param>
        /// <param name="printer">Concrete printer.</param>
        public ListCommandHandler(IFileCabinetService service, IRecordPrinter printer)
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

            if (request.Command == "list")
            {
                var list = this.service.GetRecords();

                if (list is null || list.Count == 0)
                {
                    Console.WriteLine(ThereAreNoRecordsMessage);
                    return;
                }

                this.printer.Print(list);
            }
            else
            {
                this.nextHandler.Handle(request);
            }
        }
    }
}