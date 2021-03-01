using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brochure.Abstract
{
    /// <summary>
    /// The get value.
    /// </summary>
    public interface IGetValue
    {
        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="propertyName">The property name.</param>
        /// <returns>A T.</returns>
        T GetValue<T>(string propertyName);
        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="propertyName">The property name.</param>
        /// <returns>A T.</returns>
        object GetValue(string propertyName);
    }
}
