using System;
using System.Threading;
using Kalavarda.Primitives.Process;
using Kalavarda.Primitives.WPF.Abstract;
using Room.Core.Models;
using Room.Factories;

namespace Room
{
    public class AppContext: IDisposable
    {
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        public Game Game { get; } = new Game();

        public IProcessor Processor { get; }

        public IChildUiElementFactory ChildUiElementFactory { get; } = new UiElementFactory();

        public AppContext()
        {
            Processor = new MultiProcessor(60, _cancellationTokenSource.Token) { Paused = true };
        }

        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
        }
    }
}
