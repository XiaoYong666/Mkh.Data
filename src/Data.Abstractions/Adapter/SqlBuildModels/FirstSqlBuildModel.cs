namespace Mkh.Data.Abstractions.Adapter.SqlBuildModels
{
    /// <summary>
    /// 查询第一条SQL语句构造模型
    /// </summary>
    public struct FirstSqlBuildModel
    {
        /// <summary>
        /// 数据库版本
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// 查询列
        /// </summary>
        public string Select { get; set; }

        /// <summary>
        /// 表名称
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 查询条件
        /// </summary>
        public string Where { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public string Sort { get; set; }

        /// <summary>
        /// 分组
        /// </summary>
        public string GroupBy { get; set; }

        /// <summary>
        /// 分组过滤条件
        /// </summary>
        public string Having { get; set; }
    }
}
