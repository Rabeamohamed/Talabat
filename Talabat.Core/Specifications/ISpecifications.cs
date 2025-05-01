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
    }
}
