namespace FoodStore.Common;

public class QueryParameters
{
    private const int MaxPageSize = 50;
    private int _pageSize = 10;

    public string SortOrder { get; set; } = "asc";
    public int Page { get; set; } = 1;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = Math.Min(value, MaxPageSize);
    }
}
