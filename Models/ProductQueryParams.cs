public class ProductQueryParams
{
    public string? name{ get; set; }

    public int? minSize{ get; set; }

    public int? maxSize{ get; set; }

    public string? color{ get; set; }

    public string? category{ get; set; }

    public string? sortOrder{ get; set; }

    public string? sortField{ get; set; }

    private int _pageNumber = 1;
    public int pageNumber{
        get{ return _pageNumber; }
        set{ _pageNumber = value; }
    }

    private int _pageSize = 10;
    public int pageSize{
        get{ return _pageSize; }
        set{ _pageSize = value; }
    }
}
