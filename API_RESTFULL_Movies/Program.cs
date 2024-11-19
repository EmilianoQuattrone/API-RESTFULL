using AutoMapper;
using Data.Context;
using Microsoft.EntityFrameworkCore;
using MoviesMapper.Mapper;
using Repository.IRepository;
using RepositoryPattern.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionSQL"),
    // Aquí se especifica el ensamblado de migraciones
    b => b.MigrationsAssembly("API_RESTFULL_Movies")));

// Agregamos los Repositorios
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

// Agregar AutoMapper
MapperConfiguration mapperConfig = new MapperConfiguration(cfg =>
{
    cfg.AddProfile(new Movies());
});

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

app.UseAuthorization();

app.MapControllers();

app.Run();