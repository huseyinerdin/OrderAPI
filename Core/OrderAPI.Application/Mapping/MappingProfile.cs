using AutoMapper;
using OrderAPI.Application.DTOs;
using OrderAPI.Domain.Entities;

namespace OrderAPI.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<OrderDetail, ProductDetail>().ReverseMap();
            CreateMap<CreateOrderRequest, Order>().ReverseMap();
        }
    }
}
