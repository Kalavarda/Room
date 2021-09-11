using System;
using System.Threading;
using Kalavarda.Primitives.Process;
using Room.Core.Models;

namespace Room
{
    public class AppContext: IDisposable
    {
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        public Game Game { get; } = new Game();

        public IProcessor Processor { get; }

        public AppContext()
        {
            Processor = new MultiProcessor(60, _cancellationTokenSource.Token);
        }

        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
        }
    }
}
