using System.Net;

namespace Domain.Response;

public class PaginationResponse<T> : Response<T>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public int TotalRecords { get; set; }

    public PaginationResponse(HttpStatusCode statusCode, string massage) : base(statusCode, massage)
    { }

    public PaginationResponse(int pageNumber, int pageSize, int totalRecords, T data) : base(data)
    {
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalPages = (int)Math.Ceiling((double)totalRecords / PageSize);
        TotalRecords = totalRecords;
    }
}