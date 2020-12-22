using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Mkh.Data.Abstractions.Pagination;

namespace Mkh.Data.Abstractions.Queryable
{
    /// <summary>
    /// 查询构造器
    /// </summary>
    public interface IQueryable
    {
        #region ==List==

        /// <summary>
        /// 查询列表，返回Dynamic类型
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<dynamic>> ListDynamic();

        /// <summary>
        /// 查询列表，返回指定类型
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        Task<IEnumerable<TResult>> List<TResult>();

        #endregion

        #region ==Reader==

        /// <summary>
        /// 查询列表，返回IDataReader
        /// </summary>
        /// <returns></returns>
        Task<IDataReader> Reader();

        #endregion

        #region ==Pagination==

        /// <summary>
        /// 分页查询，返回Dynamic类型
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<dynamic>> PaginationDynamic();

        /// <summary>
        /// 分页查询，返回Dynamic类型
        /// </summary>
        /// <param name="paging">分页对象</param>
        /// <returns></returns>
        Task<IEnumerable<dynamic>> PaginationDynamic(Paging paging);

        /// <summary>
        /// 分页查询，返回指定类型
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<TResult>> Pagination<TResult>();

        /// <summary>
        /// 分页查询，返回指定类型
        /// </summary>
        /// <param name="paging">分页对象</param>
        /// <returns></returns>
        Task<IEnumerable<TResult>> Pagination<TResult>(Paging paging);

        #endregion

        #region ==First==

        /// <summary>
        /// 查询第一条数据，返回Dynamic类型
        /// </summary>
        /// <returns></returns>
        Task<dynamic> FirstDynamic();

        /// <summary>
        /// 查询第一条数据，返回指定类型
        /// </summary>
        /// <returns></returns>
        Task<TResult> First<TResult>();

        #endregion

        #region ==Count==

        /// <summary>
        /// 查询数量
        /// </summary>
        /// <returns></returns>
        Task<long> Count();

        #endregion

        #region ==Exists==

        /// <summary>
        /// 判断是否存在
        /// </summary>
        /// <returns></returns>
        Task<bool> Exists();

        #endregion

        #region ==Function==

        /// <summary>
        /// 获取最大值
        /// </summary>
        /// <returns></returns>
        Task<TResult> Max<TResult>();

        /// <summary>
        /// 获取最小值
        /// </summary>
        /// <returns></returns>
        Task<TResult> Min<TResult>();

        /// <summary>
        /// 求和
        /// </summary>
        /// <returns></returns>
        Task<TResult> Sum<TResult>();

        /// <summary>
        /// 求平均值
        /// </summary>
        /// <returns></returns>
        Task<TResult> Avg<TResult>();

        #endregion

        #region ==Delete==

        /// <summary>
        /// 删除
        /// <para>数据不存在也是返回true</para>
        /// </summary>
        /// <returns></returns>
        Task<bool> Delete();

        /// <summary>
        /// 删除数据返回影响条数
        /// </summary>
        /// <returns></returns>
        Task<int> DeleteWithAffectedNum();

        #endregion

        #region ==SoftDelete==

        /// <summary>
        /// 软删除
        /// <para>数据不存在也是返回true</para>
        /// </summary>
        /// <returns></returns>
        Task<bool> SoftDelete();

        /// <summary>
        /// 软删除,返回影响条数
        /// </summary>
        /// <returns></returns>
        Task<int> SoftDeleteWithAffectedNum();

        #endregion

        #region ==SQL==

        /// <summary>
        /// 获取Sql语句
        /// </summary>
        /// <returns></returns>
        string Sql();

        /// <summary>
        /// 获取Sql语句并返回参数
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        string Sql(out IQueryParameters parameters);

        /// <summary>
        /// 获取Sql语句并使用指定的参数集
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        string Sql(IQueryParameters parameters);

        #endregion
    }
}
