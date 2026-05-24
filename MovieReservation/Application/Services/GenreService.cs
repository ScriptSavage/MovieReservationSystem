using Application.Abstraction;
using Application.Dto.Genre;
using Application.Exceptions;
using Domain.Abstractions;
using Domain.Entities;
using FluentValidation;

namespace Application.Services;

public class GenreService : IGenreService
{
    private readonly IGenreRepository _genreRepository;
    private readonly IValidator<GenreDto.Request> _genreDtoValidator;
    private readonly IUnitOfWork _unitOfWork;

    public GenreService(IGenreRepository genreRepository,
        IValidator<GenreDto.Request> genreDtoValidator,
        IUnitOfWork unitOfWork)
    {
        _genreRepository = genreRepository;
        _genreDtoValidator = genreDtoValidator;
        _unitOfWork = unitOfWork;
    }

    public async Task AddGenre(GenreDto.Request genre)
    {
        await _genreDtoValidator.ValidateAsync(genre);
        var newGenre = new Genre()
        {
            GenreName = genre.Name
        };
        await _genreRepository.AddNewGenre(newGenre);
    }

    public async Task<GenreDto.Response> GetGenre(long id)
    {
        var genre =  await _genreRepository.GetGenre(id);
        
        return new GenreDto.Response(genre.GenreName);
    }

    public async Task<IEnumerable<GenreDto.Response>> GetAllGenres()
    {
        var genreList = await _genreRepository.GetGenres();

        return genreList.Select(e => new GenreDto.Response(e.GenreName)).ToList();
    }

    public async Task RemoveGenre(long genreId)
    {
        await _genreRepository.DeleteGenre(genreId);
    }

    public async Task UpdateGenre(long id, GenreDto.Request dto)
    {
        await _genreDtoValidator.ValidateAsync(dto);
        
        var genre = await _genreRepository.GetGenre(id);
        
        if (genre == null)
        {
            throw new DoesNotExistsException("Genre not found");
        }

        genre.GenreName = dto.Name;

        await _unitOfWork.SaveChangesAsync(CancellationToken.None);
    }
}