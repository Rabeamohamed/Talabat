﻿using AutoMapper;
using Talabat.APIs.DTOs;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.APIs.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDto>()
                   .ForMember(d => d.ProductType , O => O.MapFrom(S => S.ProductType.Name))
                   .ForMember(d => d.ProductBrand, O => O.MapFrom(S => S.ProductBrand.Name ))
                   .ForMember(d => d.PictureUrl, O => O.MapFrom<ProductPictureUrlResolve>());

            CreateMap<Core.Entities.Identity.Address, AddressDto>().ReverseMap();
            CreateMap<CustomerBasketDto, CustomerBasket>();
            CreateMap<BasketItemDto, BasketItem>();
            CreateMap<AddressDto, Core.Entities.Order_Aggregate.Address>();

            CreateMap<Order, OrderToReturnDto>()
                .ForMember(d => d.DeliveryMethod, O => O.MapFrom(S => S.DeliveryMethod.ShortName))
                .ForMember(d => d.DeliveryMethodCost, O => O.MapFrom(S => S.DeliveryMethod.Cost));
        
            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(d => d.ProductId,  O => O.MapFrom(S => S.Product.ProductId))
                .ForMember(d => d.ProductName,O => O.MapFrom(S => S.Product.ProductName))
                .ForMember(d => d.PictureUrl, O => O.MapFrom(S => S.Product.PictureUrl)) 
                .ForMember(d => d.PictureUrl, O => O.MapFrom<OrderItemPictureResolver>()); 
        }
    }
}
