using System;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.Text;
using Mkh.Data.Abstractions.Adapter.SqlBuildModels;
using Mkh.Data.Abstractions.Descriptors;

namespace Mkh.Data.Abstractions.Adapter
{
    /// <summary>
    /// 数据库适配器
    /// </summary>
    public interface IDbAdapter
    {
        #region ==属性==

        /// <summary>
        /// 数据库提供器
        /// </summary>
        DbProvider Provider { get; }

        /// <summary>
        /// 左引号
        /// </summary>
        char LeftQuote { get; }

        /// <summary>
        /// 右引号
        /// </summary>
        char RightQuote { get; }

        /// <summary>
        /// 参数前缀符号
        /// </summary>
        char ParameterPrefix { get; }

        /// <summary>
        /// 获取新增ID的SQL语句
        /// </summary>
        string IdentitySql { get; }

        /// <summary>
        /// 字符串截取函数
        /// </summary>
        string FuncSubstring { get; }

        /// <summary>
        /// 字符串长度函数
        /// </summary>
        string FuncLength { get; }

        /// <summary>
        /// 转小写函数
        /// </summary>
        string FuncLower { get; }

        /// <summary>
        /// 转大写函数
        /// </summary>
        string FuncUpper { get; }

        /// <summary>
        /// SQL语句小写
        /// </summary>
        bool SqlLowerCase { get; }

        /// <summary>
        /// 布尔类型True对应的值
        /// </summary>
        string BooleanTrueValue { get; }

        /// <summary>
        /// 布尔类型Flase对应的值
        /// </summary>
        string BooleanFalseValue { get; }

        #endregion

        #region ==方法==
        
        /// <summary>
        /// 给定的值附加引号
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        string AppendQuote(string value);

        /// <summary>
        /// 给定的值附加引号
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        void AppendQuote(StringBuilder sb, string value);

        /// <summary>
        /// 附加参数
        /// </summary>
        /// <param name="parameterName">参数名</param>
        /// <returns></returns>
        string AppendParameter(string parameterName);

        /// <summary>
        /// 附加参数
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="parameterName">参数名</param>
        /// <returns></returns>
        void AppendParameter(StringBuilder sb, string parameterName);

        /// <summary>
        /// 创建新数据库连接
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <returns></returns>
        IDbConnection NewConnection(string connectionString);

        /// <summary>
        /// 分页
        /// </summary>
        /// <returns></returns>
        string GeneratePagingSql(PagingSqlBuildModel model);

        /// <summary>
        /// 生成获取第一条数据的Sql
        /// </summary>
        /// <returns></returns>
        string GenerateFirstSql(FirstSqlBuildModel model);

        /// <summary>
        /// 解析列
        /// </summary>
        /// <param name="columnDescriptor"></param>
        void ResolveColumn(IColumnDescriptor columnDescriptor);

        #endregion
    }

    /// <summary>
    /// 数据库提供器
    /// </summary>
    public enum DbProvider
    {
        /// <summary>
        /// SqlServer
        /// </summary>
        [Description("SqlServer")]
        SqlServer,
        /// <summary>
        /// MySql
        /// </summary>
        [Description("MySql")]
        MySql,
        /// <summary>
        /// SQLite
        /// </summary>
        [Description("SQLite")]
        SQLite,
        /// <summary>
        /// PostgreSQL
        /// </summary>
        [Description("PostgreSQL")]
        PostgreSQL,
        /// <summary>
        /// Oracle
        /// </summary>
        [Description("Oracle")]
        Oracle
    }
}
