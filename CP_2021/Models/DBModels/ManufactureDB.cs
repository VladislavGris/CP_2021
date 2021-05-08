using CP_2021.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP_2021.Models.DBModels
{
    [Table("Manufacture")]
    internal class ManufactureDB : Entity
    {
        [Column("M_Name", TypeName ="nvarchar(80)")]
        public string Name { get; set; }

        [Column("Letter_Num", TypeName = "nvarchar(50)")]
        public string LetterNum { get; set; }

        [Column("Specification_Num", TypeName = "nvarchar(50)")]
        public string SpecNum { get; set; }

        [Column("Production_Task_Id")]
        public int ProductionTaskId { get; set; }
        public virtual ProductionTaskDB ProductionTask { get; set; }

        public ManufactureDB Clone()
        {
            ManufactureDB manuf = new ManufactureDB();
            manuf.Name = this.Name;
            manuf.LetterNum = this.LetterNum;
            manuf.SpecNum = this.SpecNum;
            return manuf;
        }
    }
}
