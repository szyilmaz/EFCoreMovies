namespace EFCoreMovies.DTOs
{
    public class MovieFilterDTO
    {
        public string Title { get; set; }
        public int GenreId { get; set; }
        public bool InCinemas { get; set; }
        public bool UpComingReleases { get; set; }
    }
}
