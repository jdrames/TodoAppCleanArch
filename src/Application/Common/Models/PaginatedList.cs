using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Common.Models
{
    public class PaginatedList<T>
    {
        public List<T> Items { get; }

        public int PageIndex { get; }

        public int TotalPages { get; }

        public int TotalItems { get; }

        public PaginatedList(List<T> items, int count, int pageIndex, int resultsPerPage)
        {
            Items = items;
            TotalItems = count;
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)resultsPerPage);
        }

        public bool HasPreviousPage => PageIndex > 1;

        public bool HasNextPage => PageIndex < TotalPages;

        public static async Task<PaginatedList<T>> CreatePaginatedListAsync(IQueryable<T> source, int pageIndex, int resultsPerPage)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageIndex - 1) * resultsPerPage).Take(resultsPerPage).ToListAsync();

            return new PaginatedList<T>(items, count, pageIndex, resultsPerPage);
        }
    }
}
