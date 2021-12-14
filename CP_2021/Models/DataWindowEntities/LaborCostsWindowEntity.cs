using CP_2021.Models.Base;
using System;

namespace CP_2021.Models.DataWindowEntities
{
    internal class LaborCostsWindowEntity : Entity
    {
        public string Project { get; set; }
        public string Subcont { get; set; }
        public DateTime? Date { get; set; }
        public string MarkingRank { get; set; }
        public float? MarkingHours { get; set; }
        public string AssemblyRank { get; set; }
        public float? AssemblyHours { get; set; }
        public string SettingRank { get; set; }
        public float? SettingHours { get; set; }
        public float? TotalTime { get; set; }
    }
}
