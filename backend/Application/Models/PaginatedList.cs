namespace Application.Models
{
    public class PaginatedList<T>
    {
        public List<T> Items { get; }
        public int PageNumber { get; }
        public int PageSize { get; }
        public int TotalPages { get; }
        public int TotalCount { get; }


        public PaginatedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            if (pageNumber < 1) pageNumber = 1;
            else if (pageNumber > TotalPages) pageNumber = TotalPages;

            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalCount = count;
            Items = items;
        }

        public bool HasPreviousPage => PageNumber > 1;

        public bool HasNextPage => PageNumber < TotalPages;

    }
}
