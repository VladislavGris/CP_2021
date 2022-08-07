using System;

namespace CP_2021.Models.ViewEntities
{
    internal class ActForm : BaseViewEntity
    {
        public int? Count { get; set;}
        public string Complectation { get; set; }
        public string ActNumber { get; set; }
        public DateTime? ActDate { get; set; }
        public string Note { get; set; }
    }
}
