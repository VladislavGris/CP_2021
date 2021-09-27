using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP_2021.Models.ViewEntities
{
    internal class GivingStorage : BaseViewEntity
    {
        public int? Count { get; set; }
        public string Complectation { get; set; }
        public DateTime? ReceivingDate { get; set; }
    }
}
