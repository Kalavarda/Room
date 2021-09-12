using System;
using Kalavarda.Primitives;
using Kalavarda.Primitives.Abstract;
using Kalavarda.Primitives.Process;
using Kalavarda.Primitives.Skills;

namespace Room.Core.Skills
{
    public class HealSkill: ISkill, IHasKey
    {
        private readonly TimeLimiter _timeLimiter;

        public float HpChange { get; }

        public string Name => "Лечение";

        public float MaxDistance => 0;

        public ITimeLimiter TimeLimiter => _timeLimiter;
        
        public IProcess Use(ISkilled initializer)
        {
            _timeLimiter.Do(() =>
            {
                var creature = (ICreatureExt) initializer;
                creature.ChangeHP(HpChange, initializer, this);
            });
            return null;
        }

        public HealSkill(float hpChange, TimeSpan interval)
        {
            HpChange = hpChange;
            _timeLimiter = new TimeLimiter(interval);
        }

        public string Key { get; set; }
    }
}
