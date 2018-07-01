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
    /// <summary>
    /// Represents an AutoMapper Profile configured for use in the RentIt Application
    /// </summary>
    public class AppMappingProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the AppMappingProfile class
        /// </summary>
        /// <param name="moviesRepo">The Movies Repo for accessing Movie resources</param>
        public AppMappingProfile(IMoviesRepo moviesRepo)
        {
            CreateMap<Movie, MovieDto>().ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre.Name));
            CreateMap<Movie, AddMovieDto>().ForMember(dest => dest.Genre, opt => opt.MapFrom(src => src.Genre.Name));
            CreateMap<AddMovieDto, Movie>().ForMember(dest => dest.Genre, opt => opt.ResolveUsing(src => moviesRepo.GetGenreByName(src.Genre)));
        }
    }
}
