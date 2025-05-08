﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Core.Repositories
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        #region Without Specification
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);

        #endregion

        #region With Specification
        Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> Spec);

        Task<T> GetByIdWithSpecAsync(ISpecifications<T> Spec);

        Task<int> GetCountWithSpecAsync(ISpecifications<T> Spec);
        Task AddAsync(T Item);

        void Update(T Item);

        void Delete(T Item);
        #endregion

    }
}
