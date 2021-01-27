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
        Task<IList<dynamic>> ListDynamic();

        /// <summary>
        /// 查询列表，返回指定类型
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <returns></returns>
        Task<IList<TResult>> List<TResult>();

        /// <summary>
        /// 获取查询列表SQL
        /// </summary>
        /// <returns></returns>
        string ListSql();

        /// <summary>
        /// 获取查询列表SQL，并返回参数
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        string ListSql(out IQueryParameters parameters);

        /// <summary>
        /// 获取查询列表SQL，并设置参数
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        string ListSql(IQueryParameters parameters);

        /// <summary>
        /// 获取查询列表SQL，并且不使用参数化
        /// </summary>
        /// <returns></returns>
        string ListSqlNotUseParameters();

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
        Task<IList<dynamic>> PaginationDynamic();

        /// <summary>
        /// 分页查询，返回Dynamic类型
        /// </summary>
        /// <param name="paging">分页对象</param>
        /// <returns></returns>
        Task<IList<dynamic>> PaginationDynamic(Paging paging);

        /// <summary>
        /// 分页查询，返回指定类型
        /// </summary>
        /// <returns></returns>
        Task<IList<TResult>> Pagination<TResult>();

        /// <summary>
        /// 分页查询，返回指定类型
        /// </summary>
        /// <param name="paging">分页对象</param>
        /// <returns></returns>
        Task<IList<TResult>> Pagination<TResult>(Paging paging);

        /// <summary>
        /// 获取分页查询列表SQL
        /// </summary>
        /// <returns></returns>
        string PaginationSql(Paging paging);

        /// <summary>
        /// 获取分页查询列表SQL，并返回参数
        /// </summary>
        /// <param name="paging">分页信息</param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        string PaginationSql(Paging paging, out IQueryParameters parameters);

        /// <summary>
        /// 获取分页查询列表SQL，并设置参数
        /// </summary>
        /// <param name="paging">分页信息</param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        string PaginationSql(Paging paging, IQueryParameters parameters);

        /// <summary>
        /// 获取分页查询列表SQL，并且不使用参数化
        /// </summary>
        /// <returns></returns>
        string PaginationSqlNotUseParameters(Paging paging);

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

        /// <summary>
        /// 查询第一条数据的SQL
        /// </summary>
        /// <returns></returns>
        string FirstSql();

        /// <summary>
        /// 查询第一条数据的SQL，并返回参数
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        string FirstSql(out IQueryParameters parameters);

        /// <summary>
        /// 查询第一条数据的SQL，并设置参数
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        string FirstSql(IQueryParameters parameters);

        /// <summary>
        /// 查询第一条数据的SQL，并且不使用参数化
        /// </summary>
        /// <returns></returns>
        string FirstSqlNotUseParameters();

        #endregion

        #region ==Count==

        /// <summary>
        /// 查询数量
        /// </summary>
        /// <returns></returns>
        Task<long> Count();

        /// <summary>
        /// 查询数量的SQL
        /// </summary>
        /// <returns></returns>
        string CountSql();

        /// <summary>
        /// 查询数量的SQL，并返回参数
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        string CountSql(out IQueryParameters parameters);

        /// <summary>
        /// 查询数量的SQL，并设置参数
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        string CountSql(IQueryParameters parameters);

        /// <summary>
        /// 查询数量的SQL，并且不使用参数化
        /// </summary>
        /// <returns></returns>
        string CountSqlNotUseParameters();

        #endregion

        #region ==Exists==

        /// <summary>
        /// 判断是否存在
        /// </summary>
        /// <returns></returns>
        Task<bool> Exists();

        /// <summary>
        /// 判断是否存在的SQL
        /// </summary>
        /// <returns></returns>
        string ExistsSql();

        /// <summary>
        /// 判断是否存在的SQL，并返回参数
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        string ExistsSql(out IQueryParameters parameters);

        /// <summary>
        /// 判断是否存在的SQL，并设置参数
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        string ExistsSql(IQueryParameters parameters);

        /// <summary>
        /// 判断是否存在的SQL，并且不使用参数化
        /// </summary>
        /// <returns></returns>
        string ExistsSqlNotUseParameters();

        #endregion
    }
}
