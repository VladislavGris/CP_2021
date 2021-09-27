using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP_2021.Models.ViewEntities
{
    internal class InProgressView : BaseViewEntity
    {
        public int? Count { get; set; }
        public string Complectation { get; set; }
        public string MSLNumber { get; set; }
        public DateTime? GivingDate { get; set; }
        public DateTime? ProjectedDate {  get; set;}
        public string Executor1 { get; set; }
        public string Executor2 { get; set; }
    }
}
