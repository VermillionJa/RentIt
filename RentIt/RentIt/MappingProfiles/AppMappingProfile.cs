using AutoMapper;
using RentIt.Data.Entities;
using RentIt.Models.Movies;
using RentIt.Services.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentIt.MappingProfiles
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile(IMoviesRepo moviesRepo)
        {
            //Movie -> MovieDto
            CreateMap<Movie, MovieDto>().ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre.Name));

            //Movie -> AddMovieDto
            CreateMap<Movie, AddMovieDto>().ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre.Name));

            //AddMovieDto -> Movie
            CreateMap<AddMovieDto, Movie>().ForMember(dest => dest.Genre, opt => opt.ResolveUsing(src => moviesRepo.GetGenreByName(src.Genre)));
        }
    }
}
