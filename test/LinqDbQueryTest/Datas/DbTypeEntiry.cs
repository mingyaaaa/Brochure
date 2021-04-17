using Brochure.ORM.Atrributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqDbQueryTest.Datas
{
    /// <summary>
    /// The db type entiry.
    /// </summary>
    public class DbTypeEntiry
    {
        public string Id { get; set; }
        public int DInt { get; set; }
        public double DDouble { get; set; }
        public float DFloat { get; set; }
        public DateTime DDateTime { get; set; }
        public string DString { get; set; }
        public Guid DGuid { get; set; }
        public byte DByte { get; set; }

        public int? DNInt { get; set; }
        public double? DNDouble { get; set; }
        public float? DNFloat { get; set; }
        public DateTime? DNDateTime { get; set; }
        [NotNull]
        public string DNString { get; set; }
        public Guid? DNGuid { get; set; }
        public byte? DNByte { get; set; }
    }
}
