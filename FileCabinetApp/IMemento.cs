using System;
using System.Collections.ObjectModel;
using System.IO;

namespace FileCabinetApp
{
    /// <summary>
    /// Defines the model of FileCabinetService state.
    /// </summary>
    /// <typeparam name="T">Type, which stores current state.</typeparam>
    public interface IMemento<out T>
    {
        /// <summary>
        /// Gets date and time of state fixation.
        /// </summary>
        /// <returns>Date and time of state fixation.</returns>
        DateTime GetDate();

        /// <summary>
        /// Gets name of state fixation.
        /// </summary>
        /// <returns>Name of state fixation.</returns>
        string GetName();

        /// <summary>
        /// Gets state on the moment of fixation.
        /// </summary>
        /// <returns>State on the moment of fixation.</returns>
        T GetState();

        /// <summary>
        /// Saves state to CSV file.
        /// </summary>
        /// <param name="textWriter">Text writer.</param>
        public void SaveToCSV(TextWriter textWriter);

        /// <summary>
        /// Saves state to XML file.
        /// </summary>
        /// <param name="textWriter">Text writer.</param>
        public void SaveToXML(TextWriter textWriter);
    }
}