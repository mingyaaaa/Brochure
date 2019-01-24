using System.Threading;

namespace Brochure.Core.Models
{
    public class CancelTokenSource
    {
        public CancelTokenSource()
        {
            Source = new CancellationTokenSource();
            Token = Source.Token;
            Default = this;
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