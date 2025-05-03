using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
using Talabat.APIs.Helpers;
using Talabat.Core.Entities;
using Talabat.Core.Repositories;
using Talabat.Core.Specifications;
using Talabat.Repository.Data;

namespace Talabat.APIs.Controllers
{

    public class ProductsController : APIBaseController
    {
        private readonly IGenericRepository<Product> _productRepo;
        private readonly IMapper _mapper;
        private readonly IGenericRepository<ProductBrand> _brandRepo;
        private readonly IGenericRepository<ProductType> _typeRepo;

        public ProductsController(IGenericRepository<Product> ProductRepo,
            IMapper mapper,
            IGenericRepository<ProductBrand> BrandRepo,
            IGenericRepository<ProductType> TypeRepo
            )
        {
            _productRepo = ProductRepo;
            _mapper = mapper;
            _brandRepo = BrandRepo;
            _typeRepo = TypeRepo;
        }

        // Get all products
        [HttpGet]
        public async Task<ActionResult<Pagination <ProductToReturnDto>>> GetProducts([FromQuery]ProductSpecParams Params)
        {
            var spec = new ProductWithBrandAndTypeSpecifications(Params);
            var Products = await _productRepo.GetAllWithSpecAsync(spec);
           var MappedProducts = _mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(Products);
            
            ///var ReturnedOpjects = new Pagination<ProductToReturnDto>()
            ///{  
            ///    PageIndex = Params.PageIndex,
            ///    PageSize = Params.PageSize,
            ///    Data = MappedProducts
            ///}; // With Object Format 
            ///

            var CountSpec = new ProductWithFilterationForCountAsync(Params);
            var Count = await _productRepo.GetCountWithSpecAsync(CountSpec);
            return Ok(new Pagination<ProductToReturnDto>(Params.PageIndex, Params.PageSize, MappedProducts, Count));
        }

        // GET Product By ID

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ProductToReturnDto),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse),StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            var Spec = new ProductWithBrandAndTypeSpecifications(id );
            var Product = await _productRepo.GetByIdWithSpecAsync(Spec);
            if (Product is null) return NotFound(new ApiResponse(404));
            var MappedProducts = _mapper.Map<Product, ProductToReturnDto>(Product);
            //if (Product == null) return NotFound();
            return Ok(MappedProducts);
        }

        // Get All Types
        [HttpGet("Types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetTypes()
        {
            var Types = await _typeRepo.GetAllAsync();
            return Ok(Types);
        }

        // GEt All BRANDS
        [HttpGet("Brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
            var Brands = await _brandRepo.GetAllAsync();
            
            return Ok(Brands);
        }

      
    }
}
