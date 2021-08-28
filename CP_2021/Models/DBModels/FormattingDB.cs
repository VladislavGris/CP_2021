using CP_2021.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP_2021.Models.DBModels
{
    [Table("Formatting")]
    class FormattingDB : Entity
    {
        public bool IsBold { get; set; }

        public bool IsItalic {  get; set; }

        public bool IsUnderline { get; set; }

        public string FontFamily { get; set; }

        public int FontSize { get; set; }

        public Guid ProductionTaskId { get; set; }
        public virtual ProductionTaskDB ProductionTask { get; set; }

        public FormattingDB() { }

        public FormattingDB Clone()
        {
            FormattingDB formatted = new FormattingDB();
            formatted.IsBold = this.IsBold;
            return formatted;
        }
    }
}
