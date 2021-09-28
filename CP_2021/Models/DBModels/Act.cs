using CP_2021.Models.Base;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CP_2021.Models.DBModels
{
    [Table("Act")]
    internal class ActDB : Entity
    {
        public string ActNumber { get; set; }
        public DateTime? ActDate { get; set; }
        public bool ActCreation { get; set; }
        public bool ByAct { get; set; }
        public string Note { get; set; }
        public Guid ProductionTaskId { get; set; }
        public virtual ProductionTaskDB ProductionTask { get; set; }

        public ActDB() { }

        public ActDB Clone()
        {
            ActDB act = new ActDB();
            act.ActNumber = ActNumber;
            act.ActDate = ActDate;
            act.ActCreation = ActCreation;
            act.ByAct = ByAct;
            act.ByAct = ByAct;
            return act;
        }
    }
}
