using System;
using Mkh.Data.Abstractions.Filters.Entity;

namespace Mkh.Data.Core.Filters.EntityBase
{
    /// <summary>
    /// 实体基类新增过滤器
    /// </summary>
    internal class EntityBaseAddFilter : EntityAddFilter
    {
        public override void OnBefore(IEntityAddFilterContext context)
        {
            //设置实体的添加人编号、添加人姓名、添加时间
            var descriptor = context.EntityDescriptor;
            if (descriptor.IsEntityBase)
            {
                foreach (var column in descriptor.Columns)
                {
                    if (column.PropertyInfo.Name.Equals("CreatedBy"))
                    {
                        var createdBy = column.PropertyInfo.GetValue(context.Entity);
                        if (createdBy == null || (Guid)createdBy == Guid.Empty)
                        {
                            column.PropertyInfo.SetValue(context.Entity, context.DbContext.AccountResolver.AccountId);
                        }
                        continue;
                    }
                    if (column.PropertyInfo.Name.Equals("Creator"))
                    {
                        var creator = column.PropertyInfo.GetValue(context.Entity);
                        if (creator == null)
                        {
                            column.PropertyInfo.SetValue(context.Entity, context.DbContext.AccountResolver.AccountName);
                        }
                        continue;
                    }
                    if (column.PropertyInfo.Name.Equals("CreatedTime"))
                    {
                        var createdTime = column.PropertyInfo.GetValue(context.Entity);
                        if (createdTime == null)
                        {
                            column.PropertyInfo.SetValue(context.Entity, DateTime.Now);
                        }
                    }
                }
            }
        }
    }
}
