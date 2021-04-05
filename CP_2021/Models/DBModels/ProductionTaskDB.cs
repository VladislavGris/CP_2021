using CP_2021.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP_2021.Models.DBModels
{
    [Table("Production_Plan")]
    class ProductionTaskDB : Entity
    {
        [Column("Inc_Doc")]
        public string IncDoc { get; set; }
        [Column("Manag_Doc")]
        public string ManagDoc { get; set; }
        [Column("Task_Name")]
        public string Name { get; set; }
        [Column("P_Count")]
        public int? Count { get; set; }
        [Column("P_Vish_Date")]
        public DateTime VishDate { get; set; }
        [Column("P_Real_Date")]
        public DateTime RealDate { get; set; }
        //Complectation
        //Manufacture
        [Column("Expend_Num")]
        public string ExpendNum { get; set; }
        //Giving
        //InProduction
        [Column("Note")]
        public string Note { get; set; }
        [Column("Parent_ID")]
        public int? ParentId { get; set; }
        [Column("Completion")]
        public bool Completion { get; set; }
    }
}
