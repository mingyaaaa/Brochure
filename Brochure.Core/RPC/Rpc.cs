using System;
using Brochure.Abstract;

namespace Brochure.Core.RPC
{
    public class Rpc<T> : IRpcProxy<T>
    {
        public Rpc(T ins)
        {
            RpcServiceIns = ins;
        }
        public T RpcServiceIns { get; }
    }
}
