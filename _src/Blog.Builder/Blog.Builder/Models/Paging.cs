namespace Blog.Builder.Models;

public record class Paging
{
    public int ArticlesPerPage { get; set; } = 9;

    public int ArticlesCount { get; set; }

    public int CurrentPageIndex { get; set; }

    public int PageCount
    {
        get
        {
            return (int)Math.Ceiling(ArticlesCount / 9m);
        }
    }
}
