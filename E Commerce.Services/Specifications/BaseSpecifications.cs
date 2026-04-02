using E_Commerce.Domain.Contract;
using E_Commerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Services.Specifications
{
    public abstract class BaseSpecifications<TEntity, TKey> : ISpecifications<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        protected BaseSpecifications(Expression<Func<TEntity, bool>> criteriaExoression)
        {
            Criteria = criteriaExoression;
        }
        #region Includes
        public ICollection<Expression<Func<TEntity, object>>> IncludeExpressions { get; } = []; // Read only Property get its value by the constructor 

        protected void AddInclude(Expression<Func<TEntity, object>> IncludeExp)
        {
            IncludeExpressions.Add(IncludeExp);
        }
        #endregion

        #region Criteria

        public Expression<Func<TEntity, bool>> Criteria { get; }
        #endregion

        #region Sorting

        public Expression<Func<TEntity, object>> OrderBy { get; private set; }
        public Expression<Func<TEntity, object>> OrderByDescinding  { get; private set; }

     

        protected void AddOrderBy(Expression<Func<TEntity, object>> orderByExp)
        {
            OrderBy = orderByExp;
        }

        protected void AddOrderByDescinding(Expression<Func<TEntity, object>> orderByDescindingExp)
        {
            OrderByDescinding = orderByDescindingExp;
        }
        #endregion

        #region Pagination
        public int Take { get; private set; }

        public int Skip { get; private set; }

        public bool IsPagintated { get; private set; }

        protected void ApplyPagination(int PageSize , int PageIndex)
        {
            IsPagintated =true;
            Take = PageSize;
            Skip = PageSize * (PageIndex - 1); 

        }

        #endregion



    }
}
