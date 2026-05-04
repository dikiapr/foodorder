using AutoMapper;
using FoodStoreIdentity.DTOs.Request;
using FoodStoreIdentity.DTOs.Response;
using FoodStoreIdentity.Models;

namespace FoodStoreIdentity.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Category
        CreateMap<Category, CategoryResponse>();
        CreateMap<CreateCategoryRequest, Category>();
        CreateMap<UpdateCategoryRequest, Category>();

        // Product
        CreateMap<Product, ProductResponse>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null))
            .ForMember(dest => dest.CreatedByName, opt => opt.MapFrom(src => src.CreatedBy != null ? src.CreatedBy.UserName : null));
        CreateMap<CreateProductRequest, Product>();
        CreateMap<UpdateProductRequest, Product>();

        // Auth
        CreateMap<ApplicationUser, UserResponse>()
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.Role, opt => opt.Ignore());
        CreateMap<ApplicationUser, LoginResponse>()
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.Role, opt => opt.Ignore())
            .ForMember(dest => dest.Token, opt => opt.Ignore());
        CreateMap<RegisterRequest, ApplicationUser>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Username));
    }
}
