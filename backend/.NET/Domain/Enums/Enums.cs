using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    public enum Gender
    {
        NAM = 0,
        NU = 1,
        UNISEX = 2
    }

    public enum SortBy
    {
        ASCENDING = 0,
        DESCENDING = 1,
        NEWEST = 2,
        FEATURED = 3
    }

    public enum DiscountType
    {
        NONE = 0,
        PERCEN = 1,
        FIXED = 2
    }
}
