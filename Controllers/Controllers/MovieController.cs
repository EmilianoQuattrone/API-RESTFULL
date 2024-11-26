using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RepositoryPattern.IRepository.Interfaces.IMovie;

namespace Controller.Controllers
{
    [Route("api/movies")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IMapper _mapper;

        public MovieController(IMovieRepository movieRepository, IMapper mapper)
        {
            _movieRepository = movieRepository;
            _mapper = mapper;
        }
    }
}