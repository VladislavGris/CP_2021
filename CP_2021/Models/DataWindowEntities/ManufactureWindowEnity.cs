using CP_2021.Models.Base;
using System;

namespace CP_2021.Models.DataWindowEntities
{
    internal class ManufactureWindowEnity : Entity
    {
        public string Name { get; set; }
        public string LetterNum { get; set; }
        public string SpecNum { get; set; }
        public string CalendarDays { get; set; }
        public string WorkingDays { get; set; }
        public string Note { get; set; }
        public bool OnControl { get; set; }
        public bool VipiskSpec { get; set; }
        public bool ExecutionAct { get; set; }
        public DateTime? FactDate { get; set; }
        public DateTime? ExecutionTerm { get; set; }
        
    }
}
