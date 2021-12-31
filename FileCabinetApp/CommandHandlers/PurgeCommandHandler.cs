using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Purge command handler.
    /// </summary>
    public class PurgeCommandHandler : CommandHandlerBase
    {
        /// <inheritdoc/>
        public override void Handle(AppCommandRequest request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (request.Command == "purge")
            {
                if (Program.IsFileSystemStorageUsed)
                {
                    Program.FileCabinetService.Defragment();
                }
                else
                {
                    Console.WriteLine("'Purge command available only for FileCabinetFilesystemService.");
                }
            }
            else
            {
                this.nextHandler.Handle(request);
            }
        }
    }
}