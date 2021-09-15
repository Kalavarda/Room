using System;
using System.Collections.Generic;
using Kalavarda.Primitives.Abstract;
using Room.Core.Abstract;
using Room.Core.Models;

namespace Room.Core.Impl
{
    public class FinesSource: IFinesSource
    {
        private readonly Hero _hero;

        public FinesSource(Hero hero)
        {
            _hero = hero ?? throw new ArgumentNullException(nameof(hero));
        }

        public IReadOnlyDictionary<IHasName, long> Fine()
        {
            var xp = (int)MathF.Round(_hero.XP.Value * 0.1f);
            return new Dictionary<IHasName, long>
            {
                { XP.Instance, -xp }
            };
        }
    }
}
