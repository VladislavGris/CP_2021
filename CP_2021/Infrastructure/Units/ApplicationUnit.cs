using CP_2021.Data;
using CP_2021.Infrastructure.Repositories.Base;
using CP_2021.Infrastructure.Units.Base;
using CP_2021.Models.DBModels;
using CP_2021.Models.ProcedureResuts;
using CP_2021.Models.ViewEntities;
using System.Collections;
using System.Collections.Generic;

namespace CP_2021.Infrastructure.Units
{
    class ApplicationUnit : IUnitOfWork
    {
        private ApplicationContext _context;
        private BaseRepository<ProductionTaskDB> _tasks;
        private BaseRepository<ManufactureDB> _manufactures;
        private BaseRepository<GivingDB> _givings;
        private BaseRepository<InProductionDB> _productions;
        private BaseRepository<ComplectationDB> _complectations;
        private BaseRepository<UserDB> _dbUsers;
        private BaseRepository<TaskDB> _userTasks;
        private BaseRepository<ReportDB> _reports;
        private BaseRepository<HierarchyDB> _hierarchy;
        private BaseRepository<FormattingDB> _formatting;
        private BaseRepository<PaymentDB> _payment;
        private BaseRepository<LaborCostsDB> _labor;

        private BaseRepository<HeadTasks> _headTasks;
        private BaseRepository<ManufactureNames> _manufactureNames;
        private BaseRepository<NoSpecifications> _noSpec;
        private BaseRepository<SpecificationsOnControl> _onControl;
        private BaseRepository<SpecificationsInVipisk> _inVipisk;
        private BaseRepository<CoopWork> _coopWork;
        private BaseRepository<InProgressView> _inProgressView;
        private BaseRepository<DocumentationView> _docView;
        private BaseRepository<SKBCheck> _skbCheck;
        private BaseRepository<OETSStorage> _oetsStorage;
        private BaseRepository<GivingStorage> _givingStorage;
        private BaseRepository<GivingReports> _givingReports;
        private BaseRepository<ActForm> _actForm;

        private BaseRepository<SearchProcResult> _searchResults;

        public ApplicationUnit(ApplicationContext context)
        {
            _context = context;
            Tasks.Get();
            Manufactures.Get();
            Givings.Get();
            Productions.Get();
            Complectations.Get();
            DBUsers.Get();
            UserTasks.Get();
            Reports.Get();
            Hierarchy.Get();
            Formatting.Get();
            Payment.Get();
            Labor.Get();
        }

        public IRepository<ProductionTaskDB> Tasks
        {
            get
            {
                return _tasks ?? (_tasks = new BaseRepository<ProductionTaskDB>(_context));
            }
        }

        public IRepository<ManufactureDB> Manufactures
        {
            get
            {
                return _manufactures ?? (_manufactures = new BaseRepository<ManufactureDB>(_context));
            }
        }

        public IRepository<GivingDB> Givings
        {
            get
            {
                return _givings ?? (_givings = new BaseRepository<GivingDB>(_context));
            }
        }

        public IRepository<InProductionDB> Productions
        {
            get
            {
                return _productions ?? (_productions = new BaseRepository<InProductionDB>(_context));
            }
        }

        public IRepository<ComplectationDB> Complectations
        {
            get
            {
                return _complectations ?? (_complectations = new BaseRepository<ComplectationDB>(_context));
            }
        }

        public IRepository<UserDB> DBUsers
        {
            get
            {
                return _dbUsers ?? (_dbUsers = new BaseRepository<UserDB>(_context));
            }
        }

        public IRepository<TaskDB> UserTasks
        {
            get
            {
                return _userTasks ?? (_userTasks = new BaseRepository<TaskDB>(_context));
            }
        }

        public IRepository<ReportDB> Reports
        {
            get
            {
                return _reports ?? (_reports = new BaseRepository<ReportDB>(_context));
            }
        }

        public IRepository<HierarchyDB> Hierarchy
        {
            get
            {
                return _hierarchy ?? (_hierarchy = new BaseRepository<HierarchyDB>(_context));
            }
        }

        public IRepository<FormattingDB> Formatting
        {
            get
            {
                return _formatting ?? (_formatting = new BaseRepository<FormattingDB>(_context));
            }
        }

        public IRepository<PaymentDB> Payment
        {
            get
            {
                return _payment ?? (_payment = new BaseRepository<PaymentDB>(_context));
            }
        }

        public IRepository<LaborCostsDB> Labor
        {
            get
            {
                return _labor ?? (_labor = new BaseRepository<LaborCostsDB>(_context));
            }
        }

        public IRepository<NoSpecifications> NoSpec
        {
            get
            {
                return _noSpec ?? (_noSpec = new BaseRepository<NoSpecifications>(_context));
            }
        }
        public IRepository<SpecificationsOnControl> OnControl
        {
            get
            {
                return _onControl ?? (_onControl = new BaseRepository<SpecificationsOnControl>(_context));
            }
        }
        public IRepository<SpecificationsInVipisk> InVipisk
        {
            get
            {
                return _inVipisk ?? (_inVipisk = new BaseRepository<SpecificationsInVipisk>(_context));
            }
        }
        public IRepository<CoopWork> CoopWork
        {
            get
            {
                return _coopWork ?? (_coopWork = new BaseRepository<CoopWork>(_context));
            }
        }

        public IRepository<HeadTasks> HeadTasks
        {
            get
            {
                return _headTasks ?? (_headTasks = new BaseRepository<HeadTasks>(_context));
            }
        }
        public IRepository<ManufactureNames> ManufactureNames
        {
            get
            {
                return _manufactureNames ?? (_manufactureNames = new BaseRepository<ManufactureNames>(_context));
            }
        }
        public IRepository<InProgressView> InProgress
        {
            get
            {
                return _inProgressView ?? (_inProgressView = new BaseRepository<InProgressView>(_context));
            }
        }
        public IRepository<DocumentationView> Documentation
        {
            get
            {
                return _docView ?? (_docView = new BaseRepository<DocumentationView>(_context));
            }
        }
        public IRepository<SKBCheck> SKBCheck
        {
            get
            {
                return _skbCheck ?? (_skbCheck = new BaseRepository<SKBCheck>(_context));
            }
        }

        public IRepository<OETSStorage> OETSStorage
        {
            get
            {
                return _oetsStorage ?? (_oetsStorage = new BaseRepository<OETSStorage>(_context));
            }
        }

        public IRepository<GivingStorage> GivingStorage
        {
            get
            {
                return _givingStorage ?? (_givingStorage = new BaseRepository<GivingStorage>(_context));
            }
        }

        public IRepository<GivingReports> GivingReports
        {
            get
            {
                return _givingReports ?? (_givingReports = new BaseRepository<GivingReports>(_context));
            }
        }

        public IRepository<ActForm> ActForm
        {
            get
            {
                return _actForm ?? (_actForm = new BaseRepository<ActForm>(_context));
            }
        }

        public IRepository<SearchProcResult> SearchResults
        {
            get
            {
                return _searchResults ?? (_searchResults = new BaseRepository<SearchProcResult>(_context));
            }
        }

        public void Commit()
        {
             _context.SaveChanges();
        }

        public void Refresh()
        {
            Tasks.Refresh();
            Manufactures.Refresh();
            Givings.Refresh();
            Productions.Refresh();
            Complectations.Refresh();
            DBUsers.Refresh();
            UserTasks.Refresh();
            Reports.Refresh();
            Hierarchy.Refresh();
            Formatting.Refresh();
            Payment.Refresh();
            Labor.Refresh();
        }
    }
}
