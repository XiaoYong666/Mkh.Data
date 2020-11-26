using System.Threading.Tasks;
using Mkh.Data.Abstractions.Descriptors;
using Mkh.Data.Abstractions.Entities;

namespace Mkh.Data.Abstractions
{
    /// <summary>
    /// 仓储接口
    /// </summary>
    public interface IRepository
    {
        /// <summary>
        /// 数据库上下文
        /// </summary>
        IDbContext DbContext { get; }

        /// <summary>
        /// 实体描述符
        /// </summary>
        IEntityDescriptor EntityDescriptor { get; }
    }

    /// <summary>
    /// 泛型仓储接口
    /// </summary>
    public interface IRepository<TEntity> : IRepository where TEntity : IEntity, new()
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        Task Add(TEntity entity);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> Delete(dynamic id);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        Task<bool> Update(TEntity entity);

        /// <summary>
        /// 根据主键查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TEntity> Get(dynamic id);

        /// <summary>
        /// 软删除
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        Task<bool> SoftDelete(dynamic id);

        /// <summary>
        /// 根据主键判断是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> Exists(dynamic id);
    }
}
