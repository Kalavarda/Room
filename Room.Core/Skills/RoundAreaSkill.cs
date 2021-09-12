using System;
using Kalavarda.Primitives;
using Kalavarda.Primitives.Abstract;
using Kalavarda.Primitives.Geometry;
using Kalavarda.Primitives.Process;
using Kalavarda.Primitives.Skills;
using Room.Core.Models;

namespace Room.Core.Skills
{
    public class RoundAreaSkill: ISkill
    {
        private readonly ISkillProcessFactory _processFactory;
        private readonly TimeLimiter _timeLimiter;

        public string Name => "Круг";

        public float MaxDistance { get; }
        
        /// <summary>
        /// Через сколько бахнет
        /// </summary>
        public TimeSpan WaitTime { get; }

        public ITimeLimiter TimeLimiter => _timeLimiter;

        public RoundAreaSkill(float maxDistance, TimeSpan interval, TimeSpan waitTime, ISkillProcessFactory processFactory)
        {
            _processFactory = processFactory ?? throw new ArgumentNullException(nameof(processFactory));
            MaxDistance = maxDistance;
            WaitTime = waitTime;
            _timeLimiter = new TimeLimiter(interval);
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

    public class RoundAreaProcess: IProcess
    {
        private readonly ISkilled _initializer;
        private readonly RoundAreaSkill _skill;
        private readonly Game _game;
        private readonly RoundArea _area;
        private readonly DateTime _startTime = DateTime.Now;

        public event Action<IProcess> Completed;

        public RoundAreaProcess(ISkilled initializer, RoundAreaSkill skill, Game game)
        {
            _initializer = initializer ?? throw new ArgumentNullException(nameof(initializer));
            _skill = skill ?? throw new ArgumentNullException(nameof(skill));
            _game = game ?? throw new ArgumentNullException(nameof(game));

            var childItemsOwner = (IChildItemsOwnerExt)initializer;
            var center = ((IHasBounds)_initializer).Bounds.Position;
            _area = new RoundArea(new RoundBounds(center, skill.MaxDistance), childItemsOwner.ChildItemsContainer);
            childItemsOwner.ChildItemsContainer.Add(_area);
        }

        public void Process(TimeSpan delta)
        {
            if (DateTime.Now - _startTime > _skill.WaitTime)
            {
                foreach (var b in _game.GetAllBounds())
                    if (_area.Bounds.DoesIntersect(b.Bounds))
                        if (b is ICreatureExt creature)
                            if (creature != _initializer)
                                creature.ChangeHP(-20, _initializer, _skill);
                BeforeComplete();
                Completed?.Invoke(this);
            }
        }

        public void Stop()
        {
            BeforeComplete();
        }

        private void BeforeComplete()
        {
            var childItemsOwner = (IChildItemsOwnerExt) _initializer;
            childItemsOwner.ChildItemsContainer.Remove(_area);
        }
    }

    public class RoundArea: IChildItem, IHasBounds
    {
        public BoundsF Bounds { get; }

        public IChildItemsContainer Container { get; }

        public RoundArea(RoundBounds bounds, IChildItemsContainer container)
        {
            Bounds = bounds ?? throw new ArgumentNullException(nameof(bounds));
            Container = container ?? throw new ArgumentNullException(nameof(container));
        }
    }
}
