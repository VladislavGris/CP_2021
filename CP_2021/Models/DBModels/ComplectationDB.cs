using CP_2021.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP_2021.Models.DBModels
{
    [Table("Complectation")]
    internal class ComplectationDB : Entity
    {
        [Column("Complectation", TypeName = "nvarchar(MAX)")]
        public string Complectation { get; set; }

        [Column("C_Date", TypeName = "date")]
        public DateTime? ComplectationDate { get; set; }

        [Column("Comp_Percentage", TypeName = "float")]
        public float? Percentage { get; set; }

        [Column("Production_Task_Id")]
        public Guid ProductionTaskId { get; set; }
        public virtual ProductionTaskDB ProductionTask { get; set; }

        public ComplectationDB Clone()
        {
            ComplectationDB complectation = new ComplectationDB();
            complectation.Complectation = this.Complectation;
            complectation.ComplectationDate = this.ComplectationDate;
            complectation.Percentage = this.Percentage;
            return complectation;
        }
    }
}
