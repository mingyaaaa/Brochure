using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Brochure.Abstract;
using Brochure.Abstract.Models;
using Brochure.Utils;

namespace Brochure.Core
{
    /// <summary>
    /// 默认使用的是jsonConver
    /// </summary>
    public class DefaultConverPolicy : IConverPolicy
    {
        public T2 ConverTo<T1, T2> (T1 model)
        where T1 : class
        where T2 : class
        {
            var policy = new PropertyDelegate<T1, T2> ();
            return policy.ConverTo (model);
        }
    }

    public class RecordConverPolicy : IConverPolicy
    {
        public T2 ConverTo<T1, T2> (T1 model)
        where T1 : class
        where T2 : class
        {
            if (!(model is IRecord))
                return null;
            var policy = new RecordPropertyDelegate<T2> ();
            return policy.ConverTo (model as IRecord);
        }
    }

    public class ObjectToRecordConverPolicy : IConverPolicy
    {
        public T2 ConverTo<T1, T2> (T1 model)
        where T1 : class
        where T2 : class
        {
            var policy = new ObjectToRecordDelegate<T1> ();
            return (T2) policy.ConverTo (model);
        }
    }

}