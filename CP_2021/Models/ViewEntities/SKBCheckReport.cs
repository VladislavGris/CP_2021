using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP_2021.Models.ViewEntities
{
    internal class SKBCheckReport : BaseViewEntity
    {
        public int? Count { get; set; }
        public string ManagDoc { get; set; }
        public string Complectation { get; set; }
        public string Note { get; set; }
    }
}
