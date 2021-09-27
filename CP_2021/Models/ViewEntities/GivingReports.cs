namespace CP_2021.Models.ViewEntities
{
    internal class GivingReports : BaseViewEntity
    {
        public string ManagDoc { get; set; }
        public int? Count { get; set; }
        public string Manufacturer { get; set; }
        public string SpecNum { get; set; }
        public string IncDoc { get; set; }
        public string Report { get; set; }
    }
}
