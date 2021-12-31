namespace FileCabinetApp.CommandHandlers
{
#pragma warning disable CA1051, SA1401
    /// <summary>
    /// Base command handler.
    /// </summary>
    public abstract class CommandHandlerBase : ICommandHandler
    {
        /// <summary>
        /// Next hendler.
        /// </summary>
        protected ICommandHandler nextHandler;

        /// <summary>
        /// Handles command request.
        /// </summary>
        /// <param name="request">request.</param>
        public abstract void Handle(AppCommandRequest request);

        /// <summary>
        /// Sets next hendler.
        /// </summary>
        /// <param name="commandHandler">Next hendler.</param>
        public void SetNext(ICommandHandler commandHandler)
        {
            this.nextHandler = commandHandler;
        }
    }
}