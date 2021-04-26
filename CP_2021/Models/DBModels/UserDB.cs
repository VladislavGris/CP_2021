using CP_2021.Models.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP_2021.Models.DBModels
{
    [Table("Users")]
    class UserDB : Entity
    {
        [Required]
        [Column(TypeName ="nvarchar(15)")]
        public string Login { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(15)")]
        public string Password { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string Name { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string Surname { get; set; }

        //[Column(TypeName = "date")]
        //public DateTime? Birthday { get; set; }

        [Column(TypeName = "smallint")]
        public short Position { get; set; }

        public UserDB(string login, string password, string name, string surname)
        {
            Login = login;
            Password = password;
            Name = name;
            Surname = surname;
        }
    }
}
