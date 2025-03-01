using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalabatApp.Core;
using TalabatApp.Core.Entities;
using TalabatApp.Core.Repository.Contract;
using TalabatApp.Repository.Data;
using TalabatApp.Repository.Repositories;

namespace TalabatApp.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreContext _storeContext;
        // Dictionary<string, GenericRepositories<BaseEntity>> _repositories;
        private Hashtable _repositories;

        public UnitOfWork(StoreContext storeContext)
        {
            _storeContext = storeContext;
            _repositories = new Hashtable();
        }
        public async Task<int> CompleteAsync()
        {
           return await _storeContext.SaveChangesAsync();
        }

        public async ValueTask DisposeAsync()
        {
            await _storeContext.DisposeAsync();
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            var key = typeof(TEntity).Name;
            if (!_repositories.ContainsKey(key))
            {
                var repository = new GenericRepositories<TEntity>(_storeContext) ;
                _repositories.Add(key, repository) ;
            }

            return _repositories[key] as IGenericRepository<TEntity>;
        }


    }
}
