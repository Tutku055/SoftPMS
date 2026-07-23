using SoftPMS.Application.Common.Models;
using SoftPMS.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SoftPMS.Application.Common.Extensions;

public static class RoleQueryExtensions
{
    public static IQueryable<Role> ApplyDynamicFilters(
        this IQueryable<Role> query,
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

            if (string.Equals(field, "role", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(field, "name", StringComparison.OrdinalIgnoreCase))
            {
                query = op switch
                {
                    "is" or "equals" => query.Where(r => r.Name.ToLower() == val),
                    "not" => query.Where(r => r.Name.ToLower() != val),
                    _ => query.Where(r => r.Name.ToLower() == val)
                };
            }
            else if (string.Equals(field, "description", StringComparison.OrdinalIgnoreCase))
            {
                query = op switch
                {
                    "equals" => query.Where(r => r.Description.ToLower() == val),
                    "contains" => query.Where(r => r.Description.ToLower().Contains(val)),
                    "startswith" => query.Where(r => r.Description.ToLower().StartsWith(val)),
                    "endswith" => query.Where(r => r.Description.ToLower().EndsWith(val)),
                    _ => query.Where(r => r.Description.ToLower().Contains(val))
                };
            }
            else if (string.Equals(field, "userCount", StringComparison.OrdinalIgnoreCase))
            {
                if (int.TryParse(val, out var count))
                {
                    query = op switch
                    {
                        "is" => query.Where(r => r.Users.Count(u => !u.IsDeleted) == count),
                        "morethan" => query.Where(r => r.Users.Count(u => !u.IsDeleted) > count),
                        "lessthan" => query.Where(r => r.Users.Count(u => !u.IsDeleted) < count),
                        _ => query.Where(r => r.Users.Count(u => !u.IsDeleted) == count)
                    };
                }
            }
            else if (string.Equals(field, "isActive", StringComparison.OrdinalIgnoreCase))
            {
                bool targetStatus = val switch
                {
                    "1" or "true" or "active" => true,
                    "0" or "false" or "inactive" => false,
                    _ => bool.TryParse(val, out var parsed) ? parsed : true
                };

                query = op switch
                {
                    "is" or "equals" => query.Where(r => r.IsActive == targetStatus),
                    "not" => query.Where(r => r.IsActive != targetStatus),
                    _ => query.Where(r => r.IsActive == targetStatus)
                };
            }
            else if (string.Equals(field, "quickSearch", StringComparison.OrdinalIgnoreCase))
            {
                query = query.Where(r => r.Name.ToLower().Contains(val) ||
                                         r.Description.ToLower().Contains(val));
            }
        }

        return query;
    }
}
