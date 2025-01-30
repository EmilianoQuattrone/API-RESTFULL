using AutoMapper;
using Models.DTOs.Category;
using Models.DTOs.Movie;
using Models.DTOs.User;
using Models.Entities;

namespace MoviesMapper.Mappers
{
    public class MoviesMapper : Profile
    {
        public MoviesMapper()
        {
            CreateMap<Category, CategoryDto>().ReverseMap();
            CreateMap<Category, CreateCategoryDto>().ReverseMap();

            CreateMap<Movie, MovieDto>().ReverseMap();
            CreateMap<Movie, CreateMovieDto>().ReverseMap();

            CreateMap<User, UserDto>().ReverseMap();
        }
    }
}