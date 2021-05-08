using CP_2021.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP_2021.Models.DBModels
{
    [Table("In_Production")]
    internal class InProductionDB : Entity
    {
        [Column("Number", TypeName ="nvarchar(30)")]
        public string Number { get; set; }

        [Column("Giving_Date", TypeName ="date")]
        public DateTime? GivingDate { get; set; }

        [Column("Executor_Name", TypeName ="nvarchar(80)")]
        public string ExecutorName { get; set; }

        [Column("Install_Executor_Name", TypeName ="nvarchar(80)")]
        public string ExecutorName2 { get; set; }

        [Column("Completion_Date", TypeName="date")]
        public DateTime? CompletionDate { get; set; }
        
        [Column("Production_Task_Id")]
        public int ProductionTaskId { get; set; }
        public virtual ProductionTaskDB ProductionTask { get; set; }

        public InProductionDB Clone()
        {
            InProductionDB prod = new InProductionDB();
            prod.Number = this.Number;
            prod.GivingDate = this.GivingDate;
            prod.ExecutorName = this.ExecutorName;
            prod.CompletionDate = this.CompletionDate;
            return prod;
        }
    }
}
