using CP_2021.Models.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP_2021.Models.DBModels
{
    [Table("Production_Plan")]
    internal class ProductionTaskDB : Entity
    {
        [Column("Inc_Doc", TypeName = "nvarchar(MAX)")]
        public string IncDoc { get; set; }

        [Column("Manag_Doc", TypeName = "nvarchar(MAX)")]
        public string ManagDoc { get; set; }

        [Required]
        [Column("Task_Name", TypeName = "nvarchar(MAX)")]
        public string Name { get; set; }

        [Column("P_Count")]
        public int? Count { get; set; }

        [Column("Specification_Cost", TypeName = "nvarchar(MAX)")]
        public String SpecCost { get; set; }

        [Column("P_Vish_Date", TypeName = "date")]
        public DateTime? VishDate { get; set; }

        [Column("P_Real_Date", TypeName = "date")]
        public DateTime? RealDate { get; set; }

        [Column("Expend_Num", TypeName = "nvarchar(MAX)")]
        public string ExpendNum { get; set; }

        [Column("Note", TypeName = "nvarchar(MAX)")]
        public string Note { get; set; }

        public virtual HierarchyDB MyParent { get; set; }
        public virtual List<HierarchyDB> ParentTo { get; set; }
        [Required]
        [Column("Completion", TypeName = "smallint")]
        public short Completion { get; set; }

        public virtual ComplectationDB Complectation { get; set; }
        public virtual GivingDB Giving { get; set; }
        public virtual InProductionDB InProduction { get; set; }
        public virtual ManufactureDB Manufacture { get; set; }

        public ProductionTaskDB() { }

        public ProductionTaskDB(string name)
        {
            Name = name;
            Complectation = new ComplectationDB();
            Giving = new GivingDB();
            InProduction = new InProductionDB();
            Manufacture = new ManufactureDB();
        }

        public ProductionTaskDB Clone()
        {
            ProductionTaskDB taskDB = new ProductionTaskDB();
            taskDB.IncDoc = this.IncDoc;
            taskDB.ManagDoc = this.ManagDoc;
            taskDB.Name = this.Name;
            taskDB.Count = this.Count;
            taskDB.SpecCost = this.SpecCost;
            taskDB.VishDate = this.VishDate;
            taskDB.RealDate = this.RealDate;
            taskDB.ExpendNum = this.ExpendNum;
            taskDB.Complectation = this.Complectation.Clone();
            taskDB.Giving = this.Giving.Clone();
            taskDB.InProduction = this.InProduction.Clone();
            taskDB.Manufacture = this.Manufacture.Clone();
            taskDB.Completion = this.Completion;
            taskDB.Note = this.Note;
            return taskDB;
            //var newTask = new ProductionTaskDB();
            //var task = (ProductionTaskDB)this.MemberwiseClone();
            //task.Id = 0;
            //task.Giving = (GivingDB)this.Giving.Clone();
            //this.InProduction = (InProductionDB)this.InProduction.Clone();
            //this.Manufacture = (ManufactureDB)this.Manufacture.Clone();
            //this.Complectation = (ComplectationDB)this.Complectation.Clone();
            //return (ProductionTaskDB)this.MemberwiseClone();
        }
    }
}
