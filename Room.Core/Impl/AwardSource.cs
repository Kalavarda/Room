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
        private readonly IRandom _random;

        public AwardSource(ILevelMultiplier multiplier, IRandom random)
        {
            _multiplier = multiplier ?? throw new ArgumentNullException(nameof(multiplier));
            _random = random ?? throw new ArgumentNullException(nameof(random));
        }

        public IReadOnlyDictionary<IGameItemType, long> GetAwards(IHasLevel killedBoss)
        {
            var xp = _multiplier.GetValue(100, killedBoss.Level);
            return new Dictionary<IGameItemType, long>
            {
                { GameItemTypeTypes.XP, xp },
                { GameItemTypeTypes.SmallHealthPotion, _random.Int(1, 3) }
            };
        }
    }
}
