using System.Collections.Generic;
using Kalavarda.Primitives.Abstract;

namespace Room.Core.Models
{
    public class Game
    {
        public Game()
        {
            Boss = new Boss(this);
            Hero = new Hero(this);
        }

        public Arena Arena { get; } = new Arena();

        public Hero Hero { get; }

        public Boss Boss { get; }

        public IReadOnlyCollection<IHasBounds> GetAllBounds()
        {
            return new IHasBounds[] { Boss, Hero };
        }
    }
}
