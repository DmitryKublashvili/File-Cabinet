namespace FileCabinetApp
{
    /// <summary>
    /// File cabinet default service.
    /// </summary>
    public class FileCabinetCustomService : FileCabinetService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetCustomService"/> class.
        /// </summary>
        /// <param name="recordValidator">Instance of IRecordValidator.</param>
        public FileCabinetCustomService(IRecordValidator recordValidator)
            : base(recordValidator)
        {
        }
    }
}
