using Application.Dto.Genre;

namespace Application.Dto.Movie;

public static class MovieDto
{
    public record Response(string Title, 
        string OriginalTitle, 
        string Description, 
        int DurationInMinutes, 
        DateTime ReleaseYear,
        int MinimumAgeRecruitment,
        List<GenreDto.Response> Genres);

    public record CreateMovieRequest(
        string Title,
        string OriginalTitle,
        string Description,
        int DurationInMinutes,
        DateTime ReleaseYear,
        int MinimumAgeRecruitment,
        List<long> GenresId);
    
    public record MinimumResponse( 
        string Title,
        string Description);
    
    
    public record UpdateMovieTitle(string NewTitle);
}