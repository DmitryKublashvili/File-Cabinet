namespace FileCabinetApp
{
    /// <summary>
    /// File cabinet default service.
    /// </summary>
    public class FileCabinetDefaultService : FileCabinetService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetDefaultService"/> class.
        /// </summary>
        /// <param name="recordValidator">Instance of IRecordValidator.</param>
        public FileCabinetDefaultService(IRecordValidator recordValidator)
            : base(recordValidator)
        {
        }
    }
}
