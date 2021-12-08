using CP_2021.Models.Base;
using System;

namespace CP_2021.Models.DataWindowEntities
{
    internal class ComplectationWindowEntity : Entity
    {
        public string Complectation { get; set; }
        public string StateNumber { get; set; }
        public DateTime? ComplectationDate { get; set; }
        public DateTime? OnStorageDate { get; set; }
        public double? Percentage { get; set; }
        public string Rack { get; set; }
        public string Shelf { get; set; }
        public string Note { get; set; }
    }
}
