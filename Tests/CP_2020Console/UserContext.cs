using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Text;

namespace CP_2021Console
{
    class UserContext : DbContext
    {
        public UserContext() : base("DbConnection") { }

        public DbSet<User> Users { get; set; }
    }
}
