using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CP_2021.Models.ProcedureResuts.Plan
{
    internal class Task_Hierarchy_Formatting
    {
        // ProductionTask
        public Guid Id { get; set; }
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
        [Column("Expend_Num", TypeName = "nvarchar(MAX)")]
        public string ExpendNum { get; set; }
        [Column("Note", TypeName = "nvarchar(MAX)")]
        public string Note { get; set; }
        public bool Expanded { get; set; }
        public string EditingBy { get; set; }
        [Required]
        [Column("Completion", TypeName = "smallint")]
        public short Completion { get; set; }
        // HierarchyDB
        public Guid? ParentId { get; set; }
        public Guid ChildId { get; set; }
        public int LineOrder { get; set; }
        // Formatting
        public bool IsBold { get; set; }
        public bool IsItalic { get; set; }
        public bool IsUnderline { get; set; }
        public string FontFamily { get; set; }
        public int FontSize { get; set; }
        // Complectation
        [Column("Complectation", TypeName = "nvarchar(MAX)")]
        public string Complectation { get; set; }
        [Column("Comp_Percentage", TypeName = "float")]
        public float? Percentage { get; set; }
        public string Rack { get; set; }
        public string Shelf { get; set; }
        // Giving
        [Column("G_State", TypeName = "bit")]
        public bool? State { get; set; }
        // Manufacture
        public string M_Name { get; set; }
        // Act
        public string ActNumber { get; set; }
        // In_Production
        [Column("Giving_Date", TypeName = "date")]
        public DateTime? GivingDate { get; set; }

        public int ChildrenCount { get; set; }
    }
}
