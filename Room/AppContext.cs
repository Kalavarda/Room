﻿using System;
using System.Threading;
using Kalavarda.Primitives.Process;
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

        public IAwardsSource AwardsSource { get; }

        public IFinesSource FinesSource { get; }

        public IArenaFactory ArenaFactory { get; }

        public AppContext()
        {
            var soundPlayer = new SoundPlayer();
            Game = new Game(soundPlayer);
            AwardsSource = new AwardSource(Game.Hero);
            FinesSource = new FinesSource(Game.Hero);
            Processor = new MultiProcessor(60, _cancellationTokenSource.Token);
            HeroSkillBinds = new HeroSkillBinds(Game.Hero);
            ArenaFactory = new ArenaFactory(new BossSkillProcessFactory(Game, soundPlayer));
        }

        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
        }
    }
}
