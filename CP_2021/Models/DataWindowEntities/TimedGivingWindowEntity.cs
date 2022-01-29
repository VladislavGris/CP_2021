using CP_2021.Models.Base;
using System;

namespace CP_2021.Models.DataWindowEntities
{
    internal class TimedGivingWindowEntity : Entity
    {
        public bool IsTimedGiving { get; set; }
        public bool IsSKBCheck { get; set; }
        public bool IsOECStorage { get; set; }
        public string SKBNumber { get; set; }
        public string FIO { get; set; }
        public DateTime? GivingDate { get; set; }
        public string Note { get; set; }
    }
}
