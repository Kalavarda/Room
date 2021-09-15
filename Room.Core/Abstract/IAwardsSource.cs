using System.Collections.Generic;
using Kalavarda.Primitives.Abstract;

namespace Room.Core.Abstract
{
    public interface IAwardsSource
    {
        /// <summary>
        /// Создаёт награды
        /// </summary>
        IReadOnlyDictionary<IHasName, long> GetAwards(IHasLevel killedBoss);
    }
}
