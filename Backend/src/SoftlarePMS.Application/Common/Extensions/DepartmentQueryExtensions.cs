using SoftlarePMS.Application.Common.Models;
using SoftlarePMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SoftlarePMS.Application.Common.Extensions;

public static class DepartmentQueryExtensions
{
    public static IQueryable<Department> ApplyDynamicFilters(
        this IQueryable<Department> query,
        List<FilterCriteria>? filters)
    {
        if (filters == null || !filters.Any())
            return query;

        foreach (var filter in filters)
        {
            if (string.IsNullOrWhiteSpace(filter.Value))
                continue;

            var field = filter.Field.Trim();
            var op = filter.Operator?.ToLower().Trim();
            var val = filter.Value.Trim().ToLower();

            if (string.Equals(field, "name", StringComparison.OrdinalIgnoreCase))
            {
                query = op switch
                {
                    "equals" => query.Where(d => d.Name.ToLower() == val),
                    "contains" => query.Where(d => d.Name.ToLower().Contains(val)),
                    "startswith" => query.Where(d => d.Name.ToLower().StartsWith(val)),
                    "endswith" => query.Where(d => d.Name.ToLower().EndsWith(val)),
                    _ => query.Where(d => d.Name.ToLower().Contains(val))
                };
            }
            else if (string.Equals(field, "description", StringComparison.OrdinalIgnoreCase))
            {
                query = op switch
                {
                    "equals" => query.Where(d => d.Description.ToLower() == val),
                    "contains" => query.Where(d => d.Description.ToLower().Contains(val)),
                    "startswith" => query.Where(d => d.Description.ToLower().StartsWith(val)),
                    "endswith" => query.Where(d => d.Description.ToLower().EndsWith(val)),
                    _ => query.Where(d => d.Description.ToLower().Contains(val))
                };
            }
            else if (string.Equals(field, "employeeCount", StringComparison.OrdinalIgnoreCase))
            {
                if (int.TryParse(val, out var count))
                {
                    query = op switch
                    {
                        "is" => query.Where(d => d.Employees.Count == count),
                        "morethan" => query.Where(d => d.Employees.Count > count),
                        "lessthan" => query.Where(d => d.Employees.Count < count),
                        _ => query.Where(d => d.Employees.Count == count)
                    };
                }
            }
            else if (string.Equals(field, "quickSearch", StringComparison.OrdinalIgnoreCase))
            {
                query = query.Where(d => d.Name.ToLower().Contains(val) ||
                                         d.Description.ToLower().Contains(val));
            }
        }

        return query;
    }
}
