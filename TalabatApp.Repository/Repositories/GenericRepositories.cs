using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalabatApp.Core.Entities;
using TalabatApp.Core.Repository.Contract;
using TalabatApp.Core.Specifications;
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
            if(typeof(T) == typeof(Product))
            {
                return (IEnumerable<T>) await _context.Set<Product>().Include(P => P.Brand).Include(P=>P.Category).ToListAsync();

            }
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllWithSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }

        public async Task<T> GetAsync(int id)
        {
            if (typeof(T) == typeof(Product))
            {
                return await _context.Set<Product>().Where(P => P.Id == id).Include(P => P.Brand).Include(P => P.Category).FirstOrDefaultAsync() as T ;

            }
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<T> GetWithSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_context.Set<T>(), spec);
        } 

    }
}
