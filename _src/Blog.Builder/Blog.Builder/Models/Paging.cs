namespace Blog.Builder.Models;

public record class Paging
{
    public int CardsPerPage { get; set; } = 1;
    public int CardsCount { get; set; }
    public int CurrentPageIndex { get; set; }

    public int PageCount
    {
        get
        {
            return (int)Math.Ceiling(CardsCount / (decimal)CardsPerPage);
        }
    }
}
