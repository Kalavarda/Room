using System;
using Kalavarda.Primitives;
using Kalavarda.Primitives.Abstract;
using Kalavarda.Primitives.Process;
using Kalavarda.Primitives.Skills;

namespace Room.Core.Skills
{
    public abstract class SkillBase: ISkill
    {
        private readonly TimeLimiter _timeLimiter;
        private readonly ISkillProcessFactory _processFactory;

        public abstract string Name { get; }

        public ITimeLimiter TimeLimiter => _timeLimiter;

        public float MaxDistance { get; }

        protected SkillBase(float maxDistance, TimeSpan interval, ISkillProcessFactory processFactory)
        {
            MaxDistance = maxDistance;
            _timeLimiter = new TimeLimiter(interval);
            _processFactory = processFactory ?? throw new ArgumentNullException(nameof(processFactory));
        }

        public IProcess Use(ISkilled initializer)
        {
            IProcess skillProcess = null;
            _timeLimiter.Do(() =>
            {
                if (initializer is ICreature creature)
                    if (creature.IsDead)
                        return;

                skillProcess = _processFactory.Create(initializer, this);
            });
            return skillProcess;
        }
    }
}