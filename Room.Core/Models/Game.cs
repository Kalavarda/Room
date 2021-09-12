using System;
using System.Collections.Generic;
using Kalavarda.Primitives.Abstract;
using Room.Core.Abstract;
using Room.Core.Factories;

namespace Room.Core.Models
{
    public class Game
    {
        private Arena _arena;

        public Game(ISoundPlayer soundPlayer)
        {
            Hero = new Hero(new HeroSkillProcessFactory(this, soundPlayer));
        }

        public Arena Arena
        {
            get => _arena;
            set
            {
                if (_arena == value)
                    return;

                var oldValue = _arena;
                _arena = value;

                ArenaChanged?.Invoke(this, oldValue, _arena);
            }
        }

        public event Action<Game, Arena, Arena> ArenaChanged;

        public Hero Hero { get; }

        public IReadOnlyCollection<IHasBounds> GetAllBounds()
        {
            return new IHasBounds[] { Arena.Boss, Hero };
        }

        public IReadOnlyCollection<ICreature> GetAllCreatures()
        {
            return new ICreature[] { Arena.Boss, Hero };
        }
    }
}
