using System;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Stat command handler.
    /// </summary>
    public class StatCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StatCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Some instance implemented IFileCabinetService.</param>
        public StatCommandHandler(IFileCabinetService service)
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

            if (request.Command == "stat")
            {
                (int recordsCount, int deletedRecordsCount) = this.service.GetStat();

                Console.WriteLine($"{recordsCount} record(s), deleted records count: {deletedRecordsCount}.");
            }
            else
            {
                this.nextHandler.Handle(request);
            }
        }
    }
}