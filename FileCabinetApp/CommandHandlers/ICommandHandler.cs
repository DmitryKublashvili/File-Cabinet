using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;

namespace FileCabinetApp.CommandHandlers
{
    /// <summary>
    /// Command handler interface.
    /// </summary>
    public interface ICommandHandler
    {
        /// <summary>
        /// Sets next hendler.
        /// </summary>
        /// <param name="commandHandler">Next hendler.</param>
        void SetNext(ICommandHandler commandHandler);

        /// <summary>
        /// Handles command request.
        /// </summary>
        /// <param name="request">request.</param>
        void Handle(AppCommandRequest request);
    }
}