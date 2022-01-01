using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Remove command handler.
    /// </summary>
    public class RemoveCommandHandler : CommandHandlerBase
    {
        private readonly IFileCabinetService service;

        /// <summary>
        /// Initializes a new instance of the <see cref="RemoveCommandHandler"/> class.
        /// </summary>
        /// <param name="service">Some instance implemented IFileCabinetService.</param>
        public RemoveCommandHandler(IFileCabinetService service)
        {
            this.service = service;
        }

        /// <inheritdoc/>
        public override void Handle(AppCommandRequest request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (request.Command == "remove")
            {
                if (!int.TryParse(request.Parameters, out int id))
                {
                    Console.WriteLine("Incorrect input.");
                    return;
                }

                if (!this.service.IsRecordExist(id))
                {
                    Console.WriteLine($"Record #{request.Parameters} doesn't exists.");
                    return;
                }

                this.service.RemoveRecordById(id);
                Console.WriteLine($"Record #{id} is removed.");
            }
            else
            {
                this.nextHandler.Handle(request);
            }
        }
    }
}