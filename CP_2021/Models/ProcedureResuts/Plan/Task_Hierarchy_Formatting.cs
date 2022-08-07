using CP_2021.Models.Base;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Windows.Media;

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
        public string SpecCost { get; set; }
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
        private bool _isBold;
        public bool IsBold
        {
            get => _isBold;
            set => Set(ref _isBold, value);
        }
        private bool _isItalic;
        public bool IsItalic
        {
            get => _isItalic;
            set => Set(ref _isItalic, value);
        }
        public bool IsUnderline { get; set; }
        public string FontFamily { get; set; }
        private int _fontSize;
        public int FontSize
        {
            get => _fontSize;
            set => Set(ref _fontSize, value);
        }

        private string _color;
        public string Color
        {
            get => _color;
            set => Set(ref _color, value);
        }
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
        [NotMapped]
        private bool? _state;
        [Column("G_State", TypeName = "bit")]
        public bool? State {
            get => _state;
            set => Set(ref _state, value);
        }
        // Manufacture
        [NotMapped]
        private string _mName;
        public string M_Name {
            get => _mName;
            set => Set(ref _mName, value);
        }

        private string _specNum;
        public string SpecNum
        {
            get => _specNum;
            set => Set(ref _specNum, value);
        }
        // Act
        [NotMapped]
        private string _actNumber;
        public string ActNumber {
            get=> _actNumber;
            set => Set(ref _actNumber, value);
        }
        private bool _actCreation;
        public bool ActCreation
        {
            get => _actCreation;
            set => Set(ref _actCreation, value);
        }
        private DateTime? _actDate;
        public DateTime? ActDate
        {
            get => _actDate;
            set => Set(ref _actDate, value);
        }
        private bool _byAct;
        public bool ByAct
        {
            get => _byAct;
            set => Set(ref _byAct, value);
        }
        // In_Production
        [NotMapped]
        private DateTime? _givingDate;
        [Column("Giving_Date", TypeName = "date")]
        public DateTime? GivingDate {
            get => _givingDate;
            set => Set(ref _givingDate, value);
        }

        public int ChildrenCount { get; set; }
        //TimedGiving
        [NotMapped]
        private bool _isTimedGiving;
        public bool IsTimedGiving
        {
            get => _isTimedGiving;
            set => Set(ref _isTimedGiving, value);
        }
        [NotMapped]
        private bool _isSKBCheck;
        public bool IsSKBCheck
        {
            get => _isSKBCheck;
            set => Set(ref _isSKBCheck, value);
        }
        [NotMapped]
        private bool _isOECStorage;
        public bool IsOECStorage
        {
            get => _isOECStorage;
            set => Set(ref _isOECStorage, value);
        }
    }
}
