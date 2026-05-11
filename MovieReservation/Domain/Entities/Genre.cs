namespace Domain.Entities;

public class Genre
{
    public long GenreId  { get; set; }
    public string GenreName { get; set; } = null!;


    public ICollection<Movie> Movies { get; set; } = [];

}