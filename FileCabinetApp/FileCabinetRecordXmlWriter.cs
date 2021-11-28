using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Serialization;

namespace FileCabinetApp
{
    /// <summary>
    /// Records state in XML file.
    /// </summary>
    public class FileCabinetRecordXmlWriter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordXmlWriter"/> class.
        /// </summary>
        /// <param name="textWriter">Some text writer.</param>
        public FileCabinetRecordXmlWriter(TextWriter textWriter)
        {
            this.Writer = textWriter;
        }

        /// <summary>
        /// Gets or sets and sets text writer.
        /// </summary>
        /// <value>
        /// Textwriter for writing text in file.
        /// </value>
        public TextWriter Writer { get; set; }

        /// <summary>
        /// Writes state to file in XML format.
        /// </summary>
        /// <param name="state">State for recording.</param>
        public void Write(FileCabinetServiceSnapshot state)
        {
            if (state is null)
            {
                throw new ArgumentNullException(nameof(state));
            }

            List<FileCabinetRecord> records = new List<FileCabinetRecord>(state.GetState());

            XmlSerializer xml = new XmlSerializer(records.GetType());

            xml.Serialize(this.Writer, records);
        }
    }
}
