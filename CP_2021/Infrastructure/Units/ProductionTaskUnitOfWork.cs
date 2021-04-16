﻿using CP_2021.Data;
using CP_2021.Infrastructure.Repositories.Base;
using CP_2021.Infrastructure.Units.Base;
using CP_2021.Models.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP_2021.Infrastructure.Units
{
    class ProductionTaskUnitOfWork : IUnitOfWork
    {
        private ApplicationContext _context;
        private BaseRepository<ProductionTaskDB> _tasks;
        private BaseRepository<ManufactureDB> _manufactures;
        private BaseRepository<GivingDB> _givings;
        private BaseRepository<InProductionDB> _productions;
        private BaseRepository<ComplectationDB> _complectations;

        public ProductionTaskUnitOfWork(ApplicationContext context)
        {
            _context = context;
            Tasks.Get();
            Manufactures.Get();
            Givings.Get();
            Productions.Get();
            Complectations.Get();
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

        public void Commit()
        {
            _context.SaveChanges();
        }
    }
}
