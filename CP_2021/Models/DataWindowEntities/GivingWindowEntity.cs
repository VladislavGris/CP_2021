using CP_2021.Models.Base;
using System;

namespace CP_2021.Models.DataWindowEntities
{
    internal class GivingWindowEntity : Entity
    {
        public bool State { get; set; }
        public string Bill { get; set; }
        public string Report { get; set; }
        public string ReturnReport { get; set; }
        public bool ReturnGiving { get; set; }
        public DateTime ReceivingDate { get; set; }
        public string PurchaseGoods { get; set; }
        public string Note { get; set; }
    }
}
