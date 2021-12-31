using System;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Exit command handler.
    /// </summary>
    public class ExitCommandHandler : CommandHandlerBase
    {
        private const string ExitMessage = "Exiting an application...";

        /// <inheritdoc/>
        public override void Handle(AppCommandRequest request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (request.Command == "exit")
            {
                Console.WriteLine(ExitMessage);
                Program.IsRunning = false;
            }
            else
            {
                this.nextHandler.Handle(request);
            }
        }
    }
}