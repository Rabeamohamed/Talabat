using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using Talabat.APIs.DTOs;
using Talabat.APIs.Errors;
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

        public ProductsController(IGenericRepository<Product> ProductRepo, IMapper mapper)
        {
            _productRepo = ProductRepo;
            _mapper = mapper;
        }

        // Get all products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var spec = new ProductWithBrandAndTypeSpecifications();
            var Products = await _productRepo.GetAllWithSpecAsync(spec);
            var MappedProducts = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductToReturnDto>>(Products);

            return Ok(MappedProducts );
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

    }
}
