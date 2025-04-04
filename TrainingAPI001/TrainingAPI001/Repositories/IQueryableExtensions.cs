﻿using TrainingAPI001.DTOs;

namespace TrainingAPI001.Repositories
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> Pagination<T>(this IQueryable<T> queryable,
            PaginationDTO pagination)
        {
            return queryable
                .Skip((pagination.Page - 1) * pagination.RecordsPerPage)
                .Take(pagination.RecordsPerPage);
        }
    }
}
