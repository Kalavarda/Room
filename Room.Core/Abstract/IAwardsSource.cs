using System.Collections.Generic;

namespace Room.Core.Abstract
{
    public interface IAwardsSource
    {
        /// <summary>
        /// Создаёт награды
        /// </summary>
        IReadOnlyDictionary<IGameItemType, long> GetAwards(IHasLevel killedBoss);
    }
}
