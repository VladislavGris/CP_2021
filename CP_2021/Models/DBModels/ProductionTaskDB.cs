using CP_2021.Infrastructure.Singletons;
using CP_2021.Infrastructure.Units;
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

        //public bool? ActCreation { get; set; }

        [Column("Note", TypeName = "nvarchar(MAX)")]
        public string Note { get; set; }

        public bool Expanded { get; set; }

        public string EditingBy { get; set; }

        public virtual HierarchyDB MyParent { get; set; }
        public virtual List<HierarchyDB> ParentTo { get; set; }
        [Required]
        [Column("Completion", TypeName = "smallint")]
        public short Completion { get; set; }

        public virtual ComplectationDB Complectation { get; set; }
        public virtual GivingDB Giving { get; set; }
        public virtual InProductionDB InProduction { get; set; }
        public virtual ManufactureDB Manufacture { get; set; }
        public virtual FormattingDB Formatting { get; set; }
        public virtual PaymentDB Payment { get; set; }
        public virtual LaborCostsDB LaborCosts { get; set; }
        public virtual ActDB Act { get; set; }

        public ProductionTaskDB() { }

        public ProductionTaskDB(string name)
        {
            Name = name;
            Complectation = new ComplectationDB();
            Giving = new GivingDB();
            InProduction = new InProductionDB();
            Manufacture = new ManufactureDB();
            Formatting = new FormattingDB();
            Payment = new PaymentDB();
            LaborCosts = new LaborCostsDB();
            EditingBy = "default";
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
            taskDB.Formatting = this.Formatting.Clone();
            taskDB.Payment = this.Payment.Clone();
            taskDB.LaborCosts = this.LaborCosts.Clone();
            taskDB.Completion = this.Completion;
            taskDB.Note = this.Note;
            taskDB.Act = this.Act;
            return taskDB;
        }

        public ProductionTaskDB CloneWithoutId()
        {
            ProductionTaskDB task = this.Clone();
            task.MyParent = this.MyParent;
            task.ParentTo = this.ParentTo;
            return task;
        }

        //Опускает все элементы с LineOrder больше текущего
        public void DownTaskBelow()
        {
            ApplicationUnit unit = ApplicationUnitSingleton.GetInstance().dbUnit;
            var tasksByParent = unit.Tasks.Get().Where(t => t.MyParent.ParentId == this.MyParent.ParentId && t != this).OrderBy(t => t.MyParent.LineOrder);
            foreach (var task in tasksByParent)
            {
                if (task.MyParent.LineOrder >= this.MyParent.LineOrder)
                {
                    task.MyParent.LineOrder++;
                }
            }
        }

        //Поднимает все элементы с LineOrder больше текущего
        public void UpOrderBelow()
        {
            ApplicationUnit unit = ApplicationUnitSingleton.GetInstance().dbUnit;
            var tasksByParent = unit.Tasks.Get().Where(t => t.MyParent.ParentId == this.MyParent.ParentId).OrderBy(t => t.MyParent.LineOrder);
            foreach (var task in tasksByParent)
            {
                if (task.MyParent.LineOrder > this.MyParent.LineOrder)
                {
                    task.MyParent.LineOrder--;
                }
            }
        }

        public void Remove()
        {
            ApplicationUnit unit = ApplicationUnitSingleton.GetInstance().dbUnit;
            
            if(this.ParentTo != null)
            {
                while(this.ParentTo.Count != 0)
                {
                    this.ParentTo.Last().Child.Remove();
                }
            }
            unit.Tasks.Delete(this);
            unit.Commit();
        }
    }

    enum TaskCompletion : short
    {
        NotDefined = 0,
        Documentation,
        VKOnStorage,
        CooperationWork,
        Work,
        ActOrganization,
        SKBCheck,
        OEZStorage,
        CollectedByAct,
        Storage,
        TimedExtradition
    }
}
