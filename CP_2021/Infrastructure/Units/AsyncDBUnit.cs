using CP_2021.Data;
using CP_2021.Infrastructure.Repositories.Base;
using CP_2021.Models.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP_2021.Infrastructure.Units
{
    class AsyncDBUnit
    {
        private ApplicationContext _context;
        private DbRepository<ProductionTaskDB> _tasks;
        private DbRepository<ManufactureDB> _manufactures;
        private DbRepository<GivingDB> _givings;
        private DbRepository<InProductionDB> _productions;
        private DbRepository<ComplectationDB> _complectations;
        private DbRepository<UserDB> _dbUsers;
        private DbRepository<TaskDB> _userTasks;
        private DbRepository<ReportDB> _reports;
        private DbRepository<HierarchyDB> _hierarchy;
        private DbRepository<FormattingDB> _formatting;
        private DbRepository<PaymentDB> _payment;
        private DbRepository<LaborCostsDB> _labor;

        public AsyncDBUnit(ApplicationContext context)
        {
            _context = context;
        }

        public DbRepository<ProductionTaskDB> Tasks
        {
            get
            {
                return _tasks ?? (_tasks = new DbRepository<ProductionTaskDB>(_context));
            }
        }

        public DbRepository<ManufactureDB> Manufactures
        {
            get
            {
                return _manufactures ?? (_manufactures = new DbRepository<ManufactureDB>(_context));
            }
        }

        public DbRepository<GivingDB> Givings
        {
            get
            {
                return _givings ?? (_givings = new DbRepository<GivingDB>(_context));
            }
        }

        public DbRepository<InProductionDB> Productions
        {
            get
            {
                return _productions ?? (_productions = new DbRepository<InProductionDB>(_context));
            }
        }

        public DbRepository<ComplectationDB> Complectations
        {
            get
            {
                return _complectations ?? (_complectations = new DbRepository<ComplectationDB>(_context));
            }
        }

        public DbRepository<UserDB> DBUsers
        {
            get
            {
                return _dbUsers ?? (_dbUsers = new DbRepository<UserDB>(_context));
            }
        }

        public DbRepository<TaskDB> UserTasks
        {
            get
            {
                return _userTasks ?? (_userTasks = new DbRepository<TaskDB>(_context));
            }
        }

        public DbRepository<ReportDB> Reports
        {
            get
            {
                return _reports ?? (_reports = new DbRepository<ReportDB>(_context));
            }
        }

        public DbRepository<HierarchyDB> Hierarchy
        {
            get
            {
                return _hierarchy ?? (_hierarchy = new DbRepository<HierarchyDB>(_context));
            }
        }

        public DbRepository<FormattingDB> Formatting
        {
            get
            {
                return _formatting ?? (_formatting = new DbRepository<FormattingDB>(_context));
            }
        }

        public DbRepository<PaymentDB> Payment
        {
            get
            {
                return _payment ?? (_payment = new DbRepository<PaymentDB>(_context));
            }
        }

        public DbRepository<LaborCostsDB> Labor
        {
            get
            {
                return _labor ?? (_labor = new DbRepository<LaborCostsDB>(_context));
            }
        }
    }
}
