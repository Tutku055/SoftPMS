using System;
using System.Collections.Generic;
using System.Text;

namespace SoftPMS.Application.Common.Models
{
    public sealed record FilterCriteria(
        string Field,
        string Operator,
        string? Value
    );
    
}
