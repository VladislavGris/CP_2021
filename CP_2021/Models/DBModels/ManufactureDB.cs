using CP_2021.Models.Base;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CP_2021.Models.DBModels
{
    [Table("Manufacture")]
    internal class ManufactureDB : Entity
    {
        [Column("M_Name", TypeName = "nvarchar(MAX)")]
        public string Name { get; set; }

        [Column("Letter_Num", TypeName = "nvarchar(MAX)")]
        public string LetterNum { get; set; }

        [Column("Specification_Num", TypeName = "nvarchar(MAX)")]
        public string SpecNum { get; set; }

        public bool OnControl { get; set; }

        public bool VipiskSpec { get; set; }

        public DateTime? PredictDate { get; set; }

        public DateTime? FactDate { get; set; }

        //Акт выполненных работ
        public bool ExecutionAct { get; set; }
        //Срок изготовл. по спецификации
        public DateTime? ExecutionTerm { get; set; }
        //Календарные дни
        public string CalendarDays { get; set; }
        //Рабочие дни
        public string WorkingDays { get; set; }
        //Примечание
        public string Note { get; set; }

        [Column("Production_Task_Id")]
        public Guid ProductionTaskId { get; set; }
        public virtual ProductionTaskDB ProductionTask { get; set; }

        public ManufactureDB Clone()
        {
            ManufactureDB manuf = new ManufactureDB();
            manuf.Name = this.Name;
            manuf.LetterNum = this.LetterNum;
            manuf.SpecNum = this.SpecNum;
            manuf.VipiskSpec = this.VipiskSpec;
            manuf.OnControl = this.OnControl;
            manuf.PredictDate = this.PredictDate;
            manuf.FactDate = this.FactDate;
            manuf.ExecutionAct = this.ExecutionAct;
            manuf.ExecutionTerm = this.ExecutionTerm;
            manuf.CalendarDays = this.CalendarDays;
            manuf.WorkingDays = this.WorkingDays;
            return manuf;
        }
    }
}
