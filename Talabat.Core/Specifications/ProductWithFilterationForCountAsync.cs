﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
    public class ProductWithFilterationForCountAsync : BaseSpecifications<Product>
    {
        public ProductWithFilterationForCountAsync(ProductSpecParams Params) : 
            base(P =>
             (string.IsNullOrEmpty(Params.Search) || P.Name.ToLower().Contains(Params.Search)) // Filter by Name
            &&
            (!Params.BrandId.HasValue || P.ProductBrandId == Params.BrandId) // Filter by ProductType
            &&
            (!Params.TypeId.HasValue || P.ProductTypeId == Params.TypeId) // Filter by ProductType
            )
        
        
        {
            
        }
    }
}
