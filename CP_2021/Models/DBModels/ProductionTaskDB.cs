﻿using CP_2021.Models.Base;
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
    class ProductionTaskDB : Entity
    {
        [Column("Inc_Doc", TypeName = "nvarchar(100)")]
        public string IncDoc { get; set; }

        [Column("Manag_Doc", TypeName = "nvarchar(100)")]
        public string ManagDoc { get; set; }

        [Required]
        [Column("Task_Name", TypeName = "nvarchar(300)")]
        public string Name { get; set; }

        [Column("P_Count")]
        public int? Count { get; set; }

        [Column("Specification_Cost")]
        public decimal? SpecCost { get; set; }

        [Column("P_Vish_Date", TypeName = "date")]
        public DateTime? VishDate { get; set; }

        [Column("P_Real_Date", TypeName = "date")]
        public DateTime? RealDate { get; set; }

        [Column("Expend_Num", TypeName = "nvarchar(30)")]
        public string ExpendNum { get; set; }

        [Column("Note", TypeName = "nvarchar(MAX)")]
        public string Note { get; set; }

        [Column("Parent_ID")]
        public int? ParentId { get; set; }
        public ProductionTaskDB Parent { get; set; }

        [Required]
        [Column("Completion", TypeName = "bit")]
        public bool Completion { get; set; }

        public ComplectationDB Complectation { get; set; }
        public GivingDB Giving { get; set; }
        public InProductionDB InProduction { get; set; }
        public ManufactureDB Manufacture { get; set; }
    }
}