using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications
{
    public class ProductWithBrandAndTypeSpecifications : BaseSpecifications<Product>
    {
        public ProductWithBrandAndTypeSpecifications(ProductSpecParams Params) :
            base(P =>
            (string.IsNullOrEmpty(Params.Search) || P.Name.ToLower().Contains(Params.Search)) // Filter by Name
            &&
            (!Params.BrandId.HasValue || P.ProductBrandId == Params.BrandId) // Filter by ProductType
            &&
            (!Params.TypeId.HasValue || P.ProductTypeId == Params.TypeId) // Filter by ProductType
            )
        {
            Includes.Add(P => P.ProductType);
            Includes.Add(P => P.ProductBrand);
            if (!string.IsNullOrEmpty(Params.Sort))
            {
                switch (Params.Sort)
                {
                    case "PriceAsc":
                        AddOrderBy(P => P.Price);
                        break;
                    case "PriceDesc":
                        AddOrderByDescending(P => P.Price);
                        break;
                    default:
                        AddOrderBy(P => P.Name);
                        break;
                }
            }

            ApplayPAgination(Params.PageSize * (Params.PageIndex - 1), Params.PageSize);
        }
        public ProductWithBrandAndTypeSpecifications(int id) : base(P => P.Id == id)
        {
            Includes.Add(P => P.ProductType);
            Includes.Add(P => P.ProductBrand);
        }
    }
}
