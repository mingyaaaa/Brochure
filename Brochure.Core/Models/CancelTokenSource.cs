using System.Threading;

namespace Brochure.Core.Models
{
    public class CancelTokenSource
    {
        static CancelTokenSource()
        {
            Default = GetCancelTokenSource();
        }

        private CancelTokenSource(CancellationTokenSource source)
        {
            Source = source;
            Token = source.Token;
        }

        public static CancelTokenSource Default;
        public CancellationTokenSource Source;
        public CancellationToken Token;

        public static CancelTokenSource GetCancelTokenSource()
        {
            return new CancelTokenSource(new CancellationTokenSource());
        }
    }
}