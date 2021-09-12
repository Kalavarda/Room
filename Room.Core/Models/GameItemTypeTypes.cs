using System.Diagnostics;
using Room.Core.Abstract;

namespace Room.Core.Models
{
    [DebuggerDisplay("{Name}")]
    public class GameItemTypeTypes: IGameItemType
    {
        public static IGameItemType XP { get; } = new GameItemTypeTypes("Опыт");

        public string Name { get; }

        private GameItemTypeTypes(string name)
        {
            Name = name;
        }
    }
}
