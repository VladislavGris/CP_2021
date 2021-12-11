using CP_2021.Models.Base;
using System;

namespace CP_2021.Models.DataWindowEntities
{
    internal class ConsumeActWindowEntity : Entity
    {
        public string ActNumber { get; set; }
        public DateTime? ActDate { get; set; }
        public bool ActCreation { get; set; }
        public bool ByAct { get; set; }
        public string Note { get; set; }
    }
}
