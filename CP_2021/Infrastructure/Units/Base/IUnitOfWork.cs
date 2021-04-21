using CP_2021.Infrastructure.Repositories.Base;
using CP_2021.Models.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP_2021.Infrastructure.Units.Base
{
    interface IUnitOfWork
    {
        IRepository<ProductionTaskDB> Tasks { get; }
        IRepository<ManufactureDB> Manufactures { get; }
        IRepository<GivingDB> Givings { get; }
        IRepository<InProductionDB> Productions { get; }
        IRepository<ComplectationDB> Complectations { get; }
        IRepository<UserDB> DBUsers { get; }
        IRepository<TaskDB> UserTasks { get; }
        IRepository<ReportDB> Reports { get; }
        void Commit();
    }
}
