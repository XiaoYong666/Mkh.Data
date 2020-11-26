using System;
using Mkh.Data.Abstractions.Filters.Entity;

namespace Data.Common.Test
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class CustomEntityAddFilterAttribute : Attribute, IEntityAddFilter
    {
        public void OnBefore(IEntityAddFilterContext context)
        {
        }

        public void OnAfter(IEntityAddFilterContext context)
        {
        }
    }
}
