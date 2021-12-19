using CP_2021.Models.Base;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace CP_2021.Models.ProcedureResuts
{
    internal class SearchProcResult : Entity
    {
        //public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
