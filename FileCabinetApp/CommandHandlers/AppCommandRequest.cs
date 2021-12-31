namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Command request.
    /// </summary>
    public class AppCommandRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppCommandRequest"/> class.
        /// </summary>
        /// <param name="command">Command.</param>
        /// <param name="parameters">Parameters.</param>
        public AppCommandRequest(string command, string parameters)
        {
            this.Command = command;
            this.Parameters = parameters;
        }

        /// <summary>
        /// Gets command name.
        /// </summary>
        /// <value>
        /// Command name.
        /// </value>
        public string Command { get; }

        /// <summary>
        /// Gets command parameters.
        /// </summary>
        /// <value>
        /// Command parameters.
        /// </value>
        public string Parameters { get; }
    }
}