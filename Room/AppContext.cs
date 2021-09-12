using System;
using System.Threading;
using Kalavarda.Primitives.Process;
using Kalavarda.Primitives.WPF.Abstract;
using Kalavarda.Primitives.WPF.Skills;
using Room.Core.Models;
using Room.Factories;

namespace Room
{
    public class AppContext: IDisposable
    {
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        
        public ISkillBinds HeroSkillBinds { get; }

        public Game Game { get; }

        public IProcessor Processor { get; }

        public IChildUiElementFactory ChildUiElementFactory { get; } = new UiElementFactory();

        public AppContext()
        {
            Game = new Game(new SoundPlayer());
            Processor = new MultiProcessor(60, _cancellationTokenSource.Token);
            HeroSkillBinds = new HeroSkillBinds(Game.Hero);
        }

        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
        }
    }
}
