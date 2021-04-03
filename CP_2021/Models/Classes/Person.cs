using Common.Wpf.Data;

namespace CP_2021.Models.Classes
{
    class Person : TreeGridElement
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public int? ParentId { get; set; }

        public Person(int id, string name, string position, int? pid)
        {
            Id          = id;
            Name        = name;
            Position    = position;
            ParentId    = pid;
        }
    }
}
