using Domain.Models;
namespace Application;
public class PagedCollection<T> : IPagedCollection<T>
{
    public int CollectionTotal { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public T? Collection { get; set; }
}
