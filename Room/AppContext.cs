using System;
using System.Threading;
using Kalavarda.Primitives.Abstract;
using Kalavarda.Primitives.Process;
using Kalavarda.Primitives.Utils;
using Kalavarda.Primitives.WPF.Abstract;
using Kalavarda.Primitives.WPF.Skills;
using Room.Core.Abstract;
using Room.Core.Factories;
using Room.Core.Impl;
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

        public ILevelMultiplier LevelMultiplier { get; } = new LevelMultiplier();

        public IAwardsSource AwardsSource { get; }

        public IFinesSource FinesSource { get; }

        public IArenaFactory ArenaFactory { get; }

        public IHpChanger HpChanger { get; }

        public AppContext()
        {
            var soundPlayer = new SoundPlayer();
            HpChanger = new HpChanger(LevelMultiplier);

            var heroSkillProcessFactory = new HeroSkillProcessFactory(soundPlayer, HpChanger);
            var hero = new Hero(new HeroSkillsFactory(heroSkillProcessFactory, HpChanger));
            Game = new Game(hero);
            heroSkillProcessFactory.Game = Game;

            AwardsSource = new AwardSource(LevelMultiplier, RandomImpl.Instance);
            FinesSource = new FinesSource(Game.Hero);
            Processor = new MultiProcessor(60, _cancellationTokenSource.Token);
            HeroSkillBinds = new HeroSkillBinds(Game.Hero);
            ArenaFactory = new ArenaFactory(new BossSkillProcessFactory(Game, soundPlayer, HpChanger, RandomImpl.Instance), LevelMultiplier);
        }

        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
        }
    }
}
