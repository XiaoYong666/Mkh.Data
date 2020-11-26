using System;
using System.Linq;
using System.Reflection;
using Mkh.Data.Abstractions.Adapter;
using Mkh.Data.Abstractions.Annotations;
using Mkh.Data.Abstractions.Descriptors;

namespace Mkh.Data.Core.Descriptors
{
    /// <summary>
    /// 列信息描述符
    /// </summary>
    internal class ColumnDescriptor : IColumnDescriptor
    {
        /// <summary>
        /// 列名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 属性名称
        /// </summary>
        public string PropertyName { get; }

        /// <summary>
        /// 列类型名称
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// 默认值
        /// </summary>
        public string DefaultValue { get; set; }

        /// <summary>
        /// 属性信息
        /// </summary>
        public PropertyInfo PropertyInfo { get; }

        /// <summary>
        /// 是否主键
        /// </summary>
        public bool IsPrimaryKey { get; }

        /// <summary>
        /// 长度(为0表示使用最大长度)
        /// </summary>
        public int Length { get; }

        /// <summary>
        /// 可空
        /// </summary>
        public bool Nullable { get; }

        /// <summary>
        /// 精度位数
        /// </summary>
        public int PrecisionM { get; }

        /// <summary>
        /// 精度小数
        /// </summary>
        public int PrecisionD { get; }

        public ColumnDescriptor(PropertyInfo property, IDbAdapter dbAdapter)
        {
            if (property == null)
                return;

            PropertyInfo = property;

            dbAdapter.ResolveColumn(this);

            var columnAttribute = PropertyInfo.GetCustomAttribute<ColumnAttribute>();
            if (columnAttribute != null)
            {
                Name = columnAttribute.Name;
                TypeName = columnAttribute.TypeName;
                DefaultValue = columnAttribute.DefaultValue;
            }

            if (Name.IsNull())
                Name = property.Name;

            PropertyName = property.Name;

            IsPrimaryKey = Attribute.GetCustomAttributes(property).Any(attr => attr.GetType() == typeof(KeyAttribute));

            if (!IsPrimaryKey)
            {
                IsPrimaryKey = property.Name.EqualsIgnoreCase("Id");
            }

            var lengthAtt = property.GetCustomAttribute<LengthAttribute>();
            if (lengthAtt != null)
            {
                Length = lengthAtt.Length < 0 ? 50 : lengthAtt.Length;
            }

            if (property.PropertyType.IsNullable())
            {
                Nullable = true;
            }
            else
            {
                var nullableAtt = property.GetCustomAttribute<NullableAttribute>();
                Nullable = nullableAtt != null;
            }

            var precisionAtt = property.GetCustomAttribute<PrecisionAttribute>();
            if (precisionAtt != null)
            {
                PrecisionM = precisionAtt.M;
                PrecisionD = precisionAtt.D;
            }
        }
    }
}
