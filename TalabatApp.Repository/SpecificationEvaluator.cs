using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TalabatApp.Core.Entities;
using TalabatApp.Core.Specifications;

namespace TalabatApp.Repository
{
    internal static class SpecificationEvaluator<TEntity> where TEntity : BaseEntity
    {

        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> sequence, ISpecification<TEntity> spec)
        {
            var query = sequence;


            // Filtration
            if(spec.Criteria is not null)
            {
                query = query.Where(spec.Criteria);
            }
            

            // Sorting
            if(spec.OrderBy is not null)
            {
                query = query.OrderBy(spec.OrderBy);
            }
            else if(spec.OrderByDesc is not null)
            {
                query = query.OrderByDescending(spec.OrderByDesc);
            }
           

            // InCludes => Relative Data
            query = spec.Includes.Aggregate(query,(currentQuery, includeExpression) => currentQuery.Include(includeExpression));

            return query;


        }

    }
}
