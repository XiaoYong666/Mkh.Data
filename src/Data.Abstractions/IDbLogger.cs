namespace Mkh.Data.Abstractions
{
    /// <summary>
    /// 数据库日志记录器
    /// </summary>
    public interface IDbLogger
    {
        /// <summary>
        /// 日志记录
        /// </summary>
        /// <param name="action">操作类型</param>
        /// <param name="sql">SQL语句</param>
        void Write(string action, string sql);
    }
}
