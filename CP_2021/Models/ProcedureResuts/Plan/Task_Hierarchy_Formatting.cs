using CP_2021.Models.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CP_2021.Models.ProcedureResuts.Plan
{
    internal class Task_Hierarchy_Formatting : NotifiedEntity
    {
        //// ProductionTask
        //public Guid Id { get; set; }
        [Column("Inc_Doc", TypeName = "nvarchar(MAX)")]
        public string IncDoc { get; set; }
        [NotMapped]
        private string _managDoc;
        [Column("Manag_Doc", TypeName = "nvarchar(MAX)")]
        public string ManagDoc {
            get => _managDoc;
            set => Set(ref _managDoc, value);
        }
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
        [NotMapped]
        private short _completion;
        [Required]
        [Column("Completion", TypeName = "smallint")]
        public short Completion {
            get => _completion;
            set => Set(ref _completion, value);
        }
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
        [NotMapped]
        private string _complectation;
        [Column("Complectation", TypeName = "nvarchar(MAX)")]
        public string Complectation {
            get => _complectation;
            set => Set(ref _complectation, value);
        }
        [NotMapped]
        private float? _percentage;
        [Column("Comp_Percentage", TypeName = "float")]
        public float? Percentage {
            get => _percentage;
            set => Set(ref _percentage, value);
        }
        [NotMapped]
        private string _rack;
        public string Rack {
            get => _rack;
            set => Set(ref _rack, value);
        }
        [NotMapped]
        private string _shelf;
        public string Shelf {
            get => _shelf;
            set => Set(ref _shelf, value);
        }
        // Giving
        [Column("G_State", TypeName = "bit")]
        public bool? State { get; set; }
        // Manufacture
        public string M_Name { get; set; }
        // Act
        [NotMapped]
        private string _actNumber;
        public string ActNumber {
            get=> _actNumber;
            set => Set(ref _actNumber, value);
        }
        // In_Production
        [Column("Giving_Date", TypeName = "date")]
        public DateTime? GivingDate { get; set; }

        public int ChildrenCount { get; set; }
    }
}
