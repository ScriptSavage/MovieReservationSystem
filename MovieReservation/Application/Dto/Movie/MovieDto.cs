namespace Application.Dto.Movie;

public static class MovieDto
{
    public record Response(string Title, string OriginalTitle, string Description, 
        int DurationInMinutes, DateTime ReleaseYear,int MinimumAgeRecruitment);
}