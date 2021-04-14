using Common.Wpf.Data;
using CP_2021.Models.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP_2021.Models.Classes
{
    class ProductionTask : TreeGridElement
    {
        public int Id { get; set; }
        public string IncDoc { get; set; }
        public string ManagDoc { get; set; }
        public string Name { get; set; }
        public int? DetailCount { get; set; }
        public decimal? SpecCost { get; set; }
        public DateTime? VishDate { get; set; }
        public DateTime? RealDate { get; set; }
        public string ExpendNum { get; set; }
        public string Note { get; set; }
        public int? ParentId { get; set; }
        public bool Completion { get; set; }
        public ComplectationDB Complectation { get; set; }
        public GivingDB Giving { get; set; }
        public InProductionDB InProduction { get; set; }
        public ManufactureDB Manufacture { get; set; }
    }
}
