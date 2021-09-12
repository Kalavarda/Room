using System;
using System.Collections.Generic;
using Kalavarda.Primitives.Abstract;
using Room.Core.Abstract;
using Room.Core.Models;

namespace Room.Core.Impl
{
    public class AwardSource: IAwardsSource
    {
        private readonly ILevelMultiplier _multiplier;

        public AwardSource(ILevelMultiplier multiplier)
        {
            _multiplier = multiplier ?? throw new ArgumentNullException(nameof(multiplier));
        }

        public IReadOnlyDictionary<IGameItemType, float> GetAwards(IHasLevel killedBoss)
        {
            var xp = _multiplier.GetValue(100, killedBoss.Level);
            return new Dictionary<IGameItemType, float>
            {
                { GameItemTypeTypes.XP, xp }
            };
        }
    }
}
