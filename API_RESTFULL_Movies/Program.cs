using AutoMapper;
using Data.Context;
using Microsoft.EntityFrameworkCore;
using RepositoryPattern.IRepository.Interfaces.ICategory;
using RepositoryPattern.IRepository.Interfaces.IMovie;
using RepositoryPattern.IRepository.Interfaces.IUser;
using RepositoryPattern.Repository.Category;
using RepositoryPattern.Repository.Movie;
using RepositoryPattern.Repository.User;
using Service.IServices;
using Service.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionSQL"),
    // Aqu� se especifica el ensamblado de migraciones
    b => b.MigrationsAssembly("API_RESTFULL_Movies")));

// Agregamos los Repositorios
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IMovieRepository, MovieRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IMovieService, MovieService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IUserService, UserService>();

// Agregar AutoMapper
MapperConfiguration mapperConfig = new MapperConfiguration(cfg =>
{
    cfg.AddProfile(new MoviesMapper.Mappers.MoviesMapper());
});

// CORS
builder.Services.AddCors(c => c.AddPolicy("PoliticaCors", build => {
    build.WithOrigins("https://localhost:7145").AllowAnyMethod().AllowAnyHeader();
}));

IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Soporte CORS

app.UseCors("PoliticaCors");

app.UseAuthorization();

app.MapControllers();

app.Run();