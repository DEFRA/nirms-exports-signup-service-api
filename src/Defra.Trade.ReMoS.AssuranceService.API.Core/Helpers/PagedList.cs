namespace Defra.Trade.ReMoS.AssuranceService.API.Core.Helpers;

public class PagedList<T>
{
    public List<T> Items { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public bool HasPrevious => (CurrentPage > 1);
    public bool HasNext => (CurrentPage < TotalPages);

    public PagedList()
    {
        Items = new List<T>();
    }
    public PagedList(List<T> items, int count, int pageNumber, int pageSize)
    {
        TotalCount = count;
        PageSize = pageSize;
        CurrentPage = pageNumber;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        Items = items;
    }

    //public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize) 
    //{
    //    var count = source.Count();
    //    var items = await source.Skip((pageNumber-1)*pageSize).Take(pageSize).ToListAsync();
    //    return new PagedList<T>(items, count, pageNumber, pageSize);
    //}
}
