using System.Collections.Generic;
using System.Xml.Serialization;

namespace FileCabinetApp
{
    /// <summary>
    /// Special class for XmlRoot attribut implementation.
    /// </summary>
    [XmlRoot("Records")]
    public class RecordsList
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RecordsList"/> class.
        /// </summary>
        public RecordsList()
        {
        }

        /// <summary>
        /// Gets or sets records list.
        /// </summary>
        /// <value>
        /// List of records.
        /// </value>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2227:Collection properties should be read only", Justification = "It used for deserialization.")]
        public List<FileCabinetRecord> Records { get; set; }
    }
}