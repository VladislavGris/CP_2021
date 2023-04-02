using CP_2021.Models.Base;
using System;

namespace CP_2021.Models.ProcedureResuts
{
    internal class SearchProcResult : Entity
    {
        public string Name { get; set; }
        public string RootTask { get; set; }
        public string RootSubTask { get; set; }
    }
}
