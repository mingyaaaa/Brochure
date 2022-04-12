using Brochure.ORM.Atrributes;
using System;

namespace LinqDbQueryTest.Datas
{
    /// <summary>
    /// The db type entiry.
    /// </summary>
    public class DbTypeEntiry
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the d int.
        /// </summary>
        public int DInt { get; set; }

        /// <summary>
        /// Gets or sets the d double.
        /// </summary>
        public double DDouble { get; set; }

        /// <summary>
        /// Gets or sets the d float.
        /// </summary>
        public float DFloat { get; set; }

        /// <summary>
        /// Gets or sets the d date time.
        /// </summary>
        public DateTime DDateTime { get; set; }

        /// <summary>
        /// Gets or sets the d string.
        /// </summary>
        public string DString { get; set; }

        /// <summary>
        /// Gets or sets the d guid.
        /// </summary>
        public Guid DGuid { get; set; }

        /// <summary>
        /// Gets or sets the d byte.
        /// </summary>
        public byte DByte { get; set; }

        /// <summary>
        /// Gets or sets the d n int.
        /// </summary>
        public int? DNInt { get; set; }

        /// <summary>
        /// Gets or sets the d n double.
        /// </summary>
        public double? DNDouble { get; set; }

        /// <summary>
        /// Gets or sets the d n float.
        /// </summary>
        public float? DNFloat { get; set; }

        /// <summary>
        /// Gets or sets the d n date time.
        /// </summary>
        public DateTime? DNDateTime { get; set; }

        /// <summary>
        /// Gets or sets the d n string.
        /// </summary>
        [NotNull]
        public string DNString { get; set; }

        /// <summary>
        /// Gets or sets the d n guid.
        /// </summary>
        public Guid? DNGuid { get; set; }

        /// <summary>
        /// Gets or sets the d n byte.
        /// </summary>
        public byte? DNByte { get; set; }
    }
}