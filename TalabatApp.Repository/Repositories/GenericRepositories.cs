using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalabatApp.Core.Entities;
using TalabatApp.Core.Repository.Contract;
using TalabatApp.Repository.Data;

namespace TalabatApp.Repository.Repositories
{
    public class GenericRepositories<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext _context;

        public GenericRepositories( StoreContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }
    }
}
