using System;
using System.Diagnostics;
using Room.Core.Abstract;

namespace Room.Core.Models
{
    [DebuggerDisplay("{Name}")]
    public class GameItemTypeTypes: IGameItemType
    {
        public static IGameItemType XP { get; } = new GameItemTypeTypes("Опыт");

        public static IGameItemType SmallHealthPotion { get; } = new GameItemTypeTypes("Малое зелье HP")
        {
            UseInterval = TimeSpan.FromSeconds(5)
        };

        public string Name { get; }
        
        public TimeSpan UseInterval { get; private set; }

        private GameItemTypeTypes(string name)
        {
            Name = name;
        }
    }
}
