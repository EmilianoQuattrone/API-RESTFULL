using AutoMapper;
using Models.DTOs;
using Models.Entities;

namespace MoviesMapper.Mapper
{
    public class Movies : Profile
    {
        public Movies()
        {
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Category, CreateCategoryDto>().ReverseMap();
        }
    }
}