using CP_2021.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP_2021.Models.DBModels
{
    // Трудозатраты
    [Table("LaborCosts")]
    class LaborCostsDB : Entity
    {
        // Проект
        public string Project { get; set; }
        // Субконта ?
        public string Subcont { get; set; }
        // Маркировка разряд
        public string MarkingRank{ get; set;}
        // Маркировка время, ч
        public float? MarkingHours { get; set; }
        // Сборка разряд
        public string AssemblyRank { get; set; }
        // Сборка время, ч
        public float? AssemblyHours { get; set; }
        // Настройка разряд
        public string SettingRank { get; set; }
        // Настройка время, ч
        public float? SettingHours { get; set; }
        //Дата
        public DateTime? Date { get; set; }
        // Итоговое время, сумма всех времен
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public float? TotalTime { get; private set; }

        public Guid ProductionTaskId { get; set; }
        public virtual ProductionTaskDB ProductionTask { get; set; }

        public LaborCostsDB() { }

        public LaborCostsDB Clone()
        {
            LaborCostsDB labor = new LaborCostsDB();
            labor.Project = this.Project;
            labor.Subcont = this.Subcont;
            labor.SettingRank = this.SettingRank;
            labor.MarkingRank = this.MarkingRank;
            labor.AssemblyRank = this.AssemblyRank;
            labor.Date = this.Date;
            labor.SettingHours = this.SettingHours;
            labor.AssemblyHours = this.AssemblyHours;
            labor.MarkingHours = this.MarkingHours;
            return labor;
        }
    }
}
