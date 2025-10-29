using AutoMapper;
using RecruitingChallenge.DAL.Entities;
using RecruitingChallenge.Domain.Models;

namespace RecruitingChallenge.Mapper
{
    public class MapperProfile : Profile
    {
        public  MapperProfile()
        {
            CreateMap<UserEntity, User>();

            CreateMap<OrderEntity, Order>()
                .ForMember(d => d.Client, op => op.MapFrom(src => src.Client))
                .ForMember(d => d.OrderItems, op => op.MapFrom(src => src.OrderItems));

            CreateMap<Order, OrderEntity>()
                .ForMember(d => d.Client, op => op.MapFrom(src => src.Client));

            CreateMap<OrderItemEntity, OrderItem>()
                .ForMember(d => d.ProductName, op => op.MapFrom(src => src.Product.Name))
                .ForMember(d => d.UnitPrice, op => op.MapFrom(src => src.Product.UnitPrice));

            CreateMap<ProductEntity, Product>();

            CreateMap<ClientEntity, Client>()
                .ForMember(d => d.FullName, op => op.MapFrom(src => $"{src.Name} {src.LastName}"));
        }
    }
}