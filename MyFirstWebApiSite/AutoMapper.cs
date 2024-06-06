using AutoMapper;
using Entities;
using DTOs;

namespace MyFirstWebApiSite
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<Product, ProductDTO>().ForMember(dest=>dest.CategoryName,opts=>opts.MapFrom(src=>src.Category.CategoryName)).ReverseMap();
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<Order, OrderDTO>().ReverseMap();
            CreateMap<OrderItem, OrderItemDTO>().ReverseMap();
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<UserLogin, UserLoginDTO>().ReverseMap();
        }
    }
}
