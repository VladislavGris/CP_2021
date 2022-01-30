using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP_2021.Models.ViewEntities
{
    internal class OECStorage : BaseViewEntity
    {
        public int? Count { get; set; }
        public string Rack { get; set; }
        public string Shelf { get; set; }
    }
}
