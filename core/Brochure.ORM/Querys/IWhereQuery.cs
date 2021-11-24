using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brochure.ORM.Querys
{
    /// <summary>
    /// The where query.
    /// </summary>
    public interface IWhereQuery : IQuery
    {
    }

    /// <summary>
    /// The where query.
    /// </summary>
    public interface IWhereQuery<T1> : IQuery<T1>, IWhereQuery
    {
    }

    /// <summary>
    /// The where query.
    /// </summary>
    public interface IWhereQuery<T1, T2> : IQuery<T1, T2>, IWhereQuery
    {
    }

    /// <summary>
    /// The where query.
    /// </summary>
    public interface IWhereQuery<T1, T2, T3> : IQuery<T1, T2, T3>, IWhereQuery
    {
    }

    /// <summary>
    /// The where query.
    /// </summary>
    public interface IWhereQuery<T1, T2, T3, T4> : IQuery<T1, T2, T3, T4>, IWhereQuery
    {
    }

    /// <summary>
    /// The where query.
    /// </summary>
    public interface IWhereQuery<T1, T2, T3, T4, T5> : IQuery<T1, T2, T3, T4, T5>, IWhereQuery
    {
    }
}