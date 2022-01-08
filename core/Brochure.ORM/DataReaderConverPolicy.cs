using Brochure.Abstract;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brochure.ORM
{
    /// <summary>
    /// The data reader get value.
    /// </summary>
    internal class DataReaderGetValue : IGetValue
    {
        private readonly IDataReader _reader;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataReaderGetValue"/> class.
        /// </summary>
        /// <param name="reader">The reader.</param>
        public DataReaderGetValue(IDataReader reader)
        {
            _reader = reader;
        }

        /// <summary>
        /// Gets the properties.
        /// </summary>
        public IEnumerable<string> Properties => throw new NotSupportedException();

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="propertyName">The property name.</param>
        /// <returns>A T.</returns>
        public T GetValue<T>(string propertyName)
        {
            return (T)GetValue(propertyName);
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="propertyName">The property name.</param>
        /// <returns>An object.</returns>
        public object GetValue(string propertyName)
        {
            return _reader[propertyName];
        }
    }
}