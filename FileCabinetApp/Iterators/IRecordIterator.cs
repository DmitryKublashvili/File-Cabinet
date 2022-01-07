
namespace FileCabinetApp.Iterators
{
    /// <summary>
    /// Record Iterator.
    /// </summary>
    public interface IRecordIterator
    {
        /// <summary>
        /// Gets next record.
        /// </summary>
        /// <returns>FileCabinetRecord.</returns>
        FileCabinetRecord GetNext();

        /// <summary>
        /// Reports the presence of unrecycled elements.
        /// </summary>
        /// <returns>Is has more elements.</returns>
        bool HasMore();
    }
}
