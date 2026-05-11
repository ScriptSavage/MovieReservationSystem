namespace Domain.Entities;

public sealed class Movie
{
    public long MovieId { get; set; }
    
    public string Title { get; set; } =  null!;
    public string? OriginalTitle { get; set; }
    public string? Description { get; set; }
    
    
    public int DurationInMinutes { get; set; }
    public DateTime ReleaseYear { get; set; }
    
    public int MinimumAgeRecruitment { get; set; }


    public ICollection<Genre> Genres { get; set; } = [];

}