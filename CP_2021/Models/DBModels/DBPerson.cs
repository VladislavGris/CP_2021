using CP_2021.Models.Base;
namespace CP_2021.Models
{
    class DBPerson : Entity
    {
        public virtual string Name { get; set; }
        public virtual string Position { get; set; }
        public virtual int? Parent_Id { get; set; }
    }
}
