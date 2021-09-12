using System;
using Room.Core.Abstract;
using Room.Core.Models;

namespace Room.Core.Impl
{
    public class AwardSource: IAwardsSource
    {
        private readonly Hero _hero;

        public AwardSource(Hero hero)
        {
            _hero = hero ?? throw new ArgumentNullException(nameof(hero));
        }

        public void Award()
        {
        }
    }
}
