namespace EFCoreMovies.Utilities
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> Paginate<T>(this IQueryable<T> source, int page, int recordsToTake)
        {
            return source.Skip((page - 1) * recordsToTake).Take(recordsToTake);
;        }
    }
}
