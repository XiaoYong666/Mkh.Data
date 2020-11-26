using System;
using Mkh.Data.Abstractions.Filters.Entity;

namespace Mkh.Data.Core.Filters.EntityBase
{
    /// <summary>
    /// 实体基类更新过滤器
    /// </summary>
    internal class EntityBaseUpdateFilter : EntityUpdateFilter
    {
        public override void OnBefore(IEntityUpdateFilterContext context)
        {
            //设置实体的修改人编号、修改人姓名、修改时间
            var descriptor = context.EntityDescriptor;
            if (descriptor.IsEntityBase)
            {
                foreach (var column in descriptor.Columns)
                {
                    if (column.PropertyInfo.Name.Equals("ModifiedBy"))
                    {
                        var modifiedBy = column.PropertyInfo.GetValue(context.Entity);
                        if (modifiedBy == null || (Guid)modifiedBy == Guid.Empty)
                        {
                            column.PropertyInfo.SetValue(context.Entity, context.DbContext.AccountResolver.AccountId);
                        }
                        continue;
                    }
                    if (column.PropertyInfo.Name.Equals("Modifier"))
                    {
                        var modifier = column.PropertyInfo.GetValue(context.Entity);
                        if (modifier == null)
                        {
                            column.PropertyInfo.SetValue(context.Entity, context.DbContext.AccountResolver.AccountName);
                        }
                        continue;
                    }
                    if (column.PropertyInfo.Name.Equals("ModifiedTime"))
                    {
                        var modifiedTime = column.PropertyInfo.GetValue(context.Entity);
                        if (modifiedTime == null)
                        {
                            column.PropertyInfo.SetValue(context.Entity, DateTime.Now);
                        }
                    }
                }
            }
        }
    }
}
