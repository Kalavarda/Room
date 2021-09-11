namespace Room.Core.Models
{
    public class Game
    {
        public Arena Arena { get; } = new Arena();

        public Hero Hero { get; } = new Hero();
    }
}
