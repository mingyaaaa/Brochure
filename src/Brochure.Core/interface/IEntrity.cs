
using System;
using Brochure.Core.implement;

namespace Brochure.Core
{
    /*
    映射数据库中的数据
    */
    public interface IEntrity
    {
        string TableName { get; }
        Guid Id { get; }
        IModel ConverToDataModel();
    }
}