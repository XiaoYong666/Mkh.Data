using System;
using Mkh.Data.Abstractions;

namespace Mkh.Data.Core
{
    /// <summary>
    /// 默认账户解析器
    /// </summary>
    internal class DefaultAccountResolver : IAccountResolver
    {
        public Guid? TenantId => null;

        public Guid? AccountId => null;

        public string AccountName => null;
    }
}
