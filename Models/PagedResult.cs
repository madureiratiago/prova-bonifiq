namespace ProvaPub.Models
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; } = new();
        public int TotalCount { get; set; }
        public bool HasNext { get; set; }
    }
}
