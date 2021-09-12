using System.Collections.Generic;
using Kalavarda.Primitives.Abstract;
using Room.Core.Factories;

namespace Room.Core.Models
{
    public class Game
    {
        private readonly ArenaFactory _arenaFactory;

        public Game()
        {
            _arenaFactory = new ArenaFactory(new BossSkillProcessFactory(this));

            Hero = new Hero(new HeroSkillProcessFactory(this));
            Arena = _arenaFactory.Create(1);
        }

        public Arena Arena { get; }

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
