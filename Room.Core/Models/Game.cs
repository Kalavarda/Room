namespace Room.Core.Models
{
    public class Game
    {
        public Game()
        {
            Boss = new Boss(this);
        }

        public Arena Arena { get; } = new Arena();

        public Hero Hero { get; } = new Hero();

        public Boss Boss { get; }
    }
}
