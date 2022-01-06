using System;
using FileCabinetApp.Iterators;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// List command handler.
    /// </summary>
    public class ListCommandHandler : ServiceCommandHandlerBase
    {
        private const string ThereAreNoRecordsMessage = "There are no records yet.";

        private readonly Action<IRecordIterator> printer;

        /// <summary>
        /// Initializes a new instance of the <see cref="ListCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Some instance implemented IFileCabinetService.</param>
        /// <param name="printer">Concrete printer.</param>
        public ListCommandHandler(IFileCabinetService service, Action<IRecordIterator> printer)
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
                IRecordIterator iterator = this.service.GetRecords();

                if (iterator is null || !iterator.HasMore())
                {
                    Console.WriteLine(ThereAreNoRecordsMessage);
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