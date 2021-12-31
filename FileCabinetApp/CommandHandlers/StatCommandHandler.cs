using System;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Stat command handler.
    /// </summary>
    public class StatCommandHandler : CommandHandlerBase
    {
        /// <inheritdoc/>
        public override void Handle(AppCommandRequest request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (request.Command == "stat")
            {
                (int recordsCount, int deletedRecordsCount) = Program.FileCabinetService.GetStat();

                Console.WriteLine($"{recordsCount} record(s), deleted records count: {deletedRecordsCount}.");
            }
            else
            {
                this.nextHandler.Handle(request);
            }
        }
    }
}