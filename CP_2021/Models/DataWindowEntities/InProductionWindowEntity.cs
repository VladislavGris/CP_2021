using CP_2021.Models.Base;
using System;

namespace CP_2021.Models.DataWindowEntities
{
    internal class InProductionWindowEntity : Entity
    {
        public string Number { get; set; }
        public string Executor { get; set; }
        public string InstallExecutor { get; set; }
        public DateTime GivingDate { get; set; }
        public DateTime ProjectedDate { get; set; }
        public DateTime CompletionDate { get; set; }
        public string Note { get; set; }
    }
}
