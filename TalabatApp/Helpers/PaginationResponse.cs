using Microsoft.AspNetCore.Identity;
using TalabatApp.Dtos;

namespace TalabatApp.Helpers
{
    public class PaginationResponse<T>
    {
        public PaginationResponse(int pageSize, int pageIndex,int count , IReadOnlyList<T> data)
        {
            PageSize = pageSize;
            PageIndex = pageIndex;
            Data = data;
            Count = count;
        }

        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public int Count { get; set; }
        public IReadOnlyList<T> Data { get; set; }


    }
}
