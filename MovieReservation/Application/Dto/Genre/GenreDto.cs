namespace Application.Dto.Genre;

public static class GenreDto
{
    public record Request(string Name);

    public record Response(string Name);
}