using System;
using Kalavarda.Primitives;
using Kalavarda.Primitives.Abstract;
using Kalavarda.Primitives.Geometry;
using Kalavarda.Primitives.Process;
using Kalavarda.Primitives.Skills;
using Room.Core.Abstract;
using Room.Core.Models;

namespace Room.Core.Skills
{
    public class TeleportSkill: ISkill
    {
        private readonly ISkillProcessFactory _processFactory;
        private readonly TimeLimiter _timeLimiter;

        public string Name => "Телепорт";

        public float MaxDistance { get; }

        public TimeSpan Duration { get; }

        public ITimeLimiter TimeLimiter => _timeLimiter;

        public TeleportSkill(float maxDistance, TimeSpan interval, TimeSpan duration,
            ISkillProcessFactory processFactory)
        {
            _processFactory = processFactory ?? throw new ArgumentNullException(nameof(processFactory));
            Duration = duration;
            MaxDistance = maxDistance;
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

    public class TeleportProcess: IProcess
    {
        private readonly ISkilled _initializer;
        private readonly TeleportSkill _skill;
        private readonly Arena _arena;
        private readonly PointF _startPosition;
        private readonly float _lookDirection;
        private readonly PointF _position;
        private readonly float _speed;

        public event Action<IProcess> Completed;

        public TeleportProcess(ISkilled initializer, TeleportSkill skill, Arena arena)
        {
            _initializer = initializer ?? throw new ArgumentNullException(nameof(initializer));
            _skill = skill ?? throw new ArgumentNullException(nameof(skill));
            _arena = arena ?? throw new ArgumentNullException(nameof(arena));

            _position = ((IHasBounds)_initializer).Bounds.Position;
            _startPosition = _position.DeepClone();
            _lookDirection = ((ILooking) _initializer).LookDirection.Value;
            _speed = (float)(skill.MaxDistance / skill.Duration.TotalSeconds);
        }
        
        public void Process(TimeSpan delta)
        {
            var dt = (float)delta.TotalSeconds;
            var dx = dt * _speed * MathF.Cos(_lookDirection);
            var dy = dt * _speed * MathF.Sin(_lookDirection);
            var newX = _position.X + dx;
            var newY = _position.Y + dy;

            if (!_arena.Bounds.DoesIntersect(newX, newY))
            {
                Completed?.Invoke(this);
                return;
            }

            _position.Set(newX, newY);

            if (_position.DistanceTo(_startPosition) >= _skill.MaxDistance)
                Completed?.Invoke(this);
        }

        public void Stop()
        {
        }
    }
}
