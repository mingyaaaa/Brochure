using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brochure.ORM.Querys
{
    public interface IWhereQuery : IQuery
    {
    }

    public interface IWhereQuery<T1> : IQuery<T1>, IWhereQuery
    {
    }

    public interface IWhereQuery<T1, T2> : IQuery<T1, T2>, IWhereQuery
    {
    }

    public interface IWhereQuery<T1, T2, T3> : IQuery<T1, T2, T3>, IWhereQuery
    {
    }

    public interface IWhereQuery<T1, T2, T3, T4> : IQuery<T1, T2, T3, T4>, IWhereQuery
    {
    }

    public interface IWhereQuery<T1, T2, T3, T4, T5> : IQuery<T1, T2, T3, T4, T5>, IWhereQuery
    {
    }
}