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

            if(spec.Criteria is not null)
            {
                query = query.Where(spec.Criteria);
            }


            query = spec.Includes.Aggregate(query,(currentQuery, includeExpression) => currentQuery.Include(includeExpression));

            return query;


        }

    }
}
