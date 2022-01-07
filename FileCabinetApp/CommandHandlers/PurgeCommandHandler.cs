using System;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Purge command handler.
    /// </summary>
    public class PurgeCommandHandler : ServiceCommandHandlerBase
    {
        private readonly bool isFileSystemStorageUsed;

        /// <summary>
        /// Initializes a new instance of the <see cref="PurgeCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Some instance implemented IFileCabinetService.</param>
        /// <param name="isFileSystemStorageUsed">Value that indicates wрether FileSystemStorageUsed.</param>
        public PurgeCommandHandler(IFileCabinetService service, bool isFileSystemStorageUsed)
            : base(service)
        {
            this.isFileSystemStorageUsed = isFileSystemStorageUsed;
        }

        /// <inheritdoc/>
        public override void Handle(AppCommandRequest request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (request.Command == "purge")
            {
                if (this.isFileSystemStorageUsed)
                {
                    this.service.Defragment();
                }
                else
                {
                    Console.WriteLine("'Purge command available only for FileCabinetFilesystemService.");
                }
            }
            else
            {
                if (this.nextHandler is null)
                {
                    Console.WriteLine("Incorrect command");
                }

                this.nextHandler?.Handle(request);
            }
        }
    }
}