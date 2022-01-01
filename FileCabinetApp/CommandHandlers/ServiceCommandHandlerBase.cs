namespace FileCabinetApp.CommandHandlers
{
#pragma warning disable SA1401, CA1051

    /// <summary>
    /// Base command handler.
    /// </summary>
    public abstract class ServiceCommandHandlerBase : CommandHandlerBase
    {
        /// <summary>
        /// Instance that implements the interface IFileCabinetService.
        /// </summary>
        protected readonly IFileCabinetService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceCommandHandlerBase"/> class.
        /// </summary>
        /// <param name="service">Some instance implemented IFileCabinetService.</param>
        protected ServiceCommandHandlerBase(IFileCabinetService service)
        {
            this.service = service;
        }
    }
}