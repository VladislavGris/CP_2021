using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP_2021.Models.ViewEntities
{
    internal class TimedGiving : BaseSKBViewEntity
    {
        public DateTime? GivingDate { get; set; }
        public string FIO { get; set; }
        public string Note { get; set; }
    }
}
