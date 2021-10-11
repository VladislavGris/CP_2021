using CP_2021.Models.ViewEntities;
using System;

namespace CP_2021.Models.ProcedureResuts.PaymentReports
{
    internal class PaymentFirstPart : BaseViewEntity
    {
        public string ManufactureName { get; set; }
        public string SpecNum { get; set; }
        public string ManagDoc { get; set; }
        public string SpecSum { get; set; }
        public string ProjectMan { get; set; }
        public string FirstPaymentSum { get; set; }
        public DateTime? FirstPaymentDate { get; set; }
        public string IncDoc { get; set; }
        public string Note { get; set; }
    }
}
