using System;
using Kalavarda.Primitives;
using Kalavarda.Primitives.Abstract;
using Kalavarda.Primitives.Process;
using Kalavarda.Primitives.Skills;
using Room.Core.Abstract;

namespace Room.Core.Skills
{
    public class HealSkill: ISkill, IHasKey
    {
        private readonly IHpChanger _hpChanger;
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
                _hpChanger.ApplyChange(creature, HpChange, initializer, this);
            });
            return null;
        }

        public HealSkill(float hpChange, TimeSpan interval, IHpChanger hpChanger)
        {
            _hpChanger = hpChanger ?? throw new ArgumentNullException(nameof(hpChanger));
            HpChange = hpChange;
            _timeLimiter = new TimeLimiter(interval);
        }

        public string Key { get; set; }
    }
}
