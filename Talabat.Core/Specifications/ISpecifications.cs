using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
    public interface ISpecifications<T> where T : BaseEntity
    {
        // Signature for Property for Where Condition
        public Expression<Func<T, bool>> Criteria { get; set; }

        // Signature for Property for List Of Includes
        public List<Expression<Func<T, object>>> Includes { get; set; }

        // Signature for Property for Order By

        public Expression<Func<T, object>> OrderBy { get; set; }

        // Signature for Property for Order By Descending
        public Expression<Func<T, object>> OrderByDescending { get; set; }

        public int Take { get; set; }

        public int Skip { get; set; }

        public bool IsPagingEnabled { get; set; }
    }

}
