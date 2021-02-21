using System;

namespace Brochure.Abstract
{
    public interface IPluginLoadAction
    {
        void Invoke (Guid key);
    }

    public class DefaultLoadAction : IPluginLoadAction
    {
        public void Invoke (Guid key) { }
    }
    public interface IPluginUnLoadAction
    {
        void Invoke (Guid key);
    }
    public class DefaultUnLoadAction : IPluginUnLoadAction
    {
        public void Invoke (Guid key) { }
    }
}