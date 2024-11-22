using AutoMapper;
using Models.DTOs.Category;
using Models.DTOs.Movie;
using Models.Entities;

namespace MoviesMapper.Mapper
{
    public class Movies : Profile
    {
        public Movies()
        {
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Category, CreateCategoryDto>().ReverseMap();

            CreateMap<Movie, MovieDto>().ReverseMap();
            CreateMap<Movie, CreateMovieDto>().ReverseMap();
        }
    }
}