using AutoMapper;
using Data.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RepositoryPattern.IRepository.Interfaces.ICategory;
using RepositoryPattern.IRepository.Interfaces.IMovie;
using RepositoryPattern.IRepository.Interfaces.IUser;
using RepositoryPattern.Repository.Category;
using RepositoryPattern.Repository.Movie;
using RepositoryPattern.Repository.User;
using Service.IServices;
using Service.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionSQL"),
    // Aquí se especifica el ensamblado de migraciones
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

string secretKey = builder.Configuration.GetValue<string>("ApiSettings:SecretKey");

// Autenticación.
builder.Services.AddAuthentication(

    x =>
    {
        x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }
).AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey)),
        ValidateIssuer = false,
        ValidateAudience = false,
    };
});

IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Ejemplo: \"Bearer ajsbfuwveef12e2\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});


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

// Conf para autenticación.
app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();