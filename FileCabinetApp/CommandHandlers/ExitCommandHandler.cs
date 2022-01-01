using System;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Exit command handler.
    /// </summary>
    public class ExitCommandHandler : CommandHandlerBase
    {
        private const string ExitMessage = "Exiting an application...";

        private readonly Action exitProgramm;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExitCommandHandler"/> class.
        /// </summary>
        /// <param name="exitProgramm">Delegate provides method of closing application from outside.</param>
        public ExitCommandHandler(Action exitProgramm)
        {
            this.exitProgramm = exitProgramm;
        }

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
                this.exitProgramm?.Invoke();
            }
            else
            {
                this.nextHandler.Handle(request);
            }
        }
    }
}