using System;
using Kalavarda.Primitives.Abstract;
using Kalavarda.Primitives.Geometry;
using Kalavarda.Primitives.Process;
using Kalavarda.Primitives.Skills;
using Room.Core.Abstract;
using Room.Core.Models;

namespace Room.Core.Skills
{
    public class TeleportSkill: SkillBase
    {
        public override string Name => Backward ? "Отскок назад" : "Телепорт вперёд";

        public TimeSpan Duration { get; }

        public bool Backward { get; }

        /// <summary>
        /// Invulnerability frame
        /// </summary>
        public bool InvFrame { get; }

        public TeleportSkill(float maxDistance, TimeSpan interval, TimeSpan duration, bool backward,
            bool iFrame,
            ISkillProcessFactory processFactory)
            : base(maxDistance, interval, processFactory)
        {
            Duration = duration;
            Backward = backward;
            InvFrame = iFrame;
        }
    }

    public class TeleportProcess: IProcess
    {
        private readonly ISkilled _initializer;
        private readonly TeleportSkill _skill;
        private readonly Arena _arena;
        private readonly PointF _startPosition;
        private readonly float _shiftDirection;
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
            _speed = (float)(skill.MaxDistance / skill.Duration.TotalSeconds);

            _shiftDirection = ((ILooking)_initializer).LookDirection.Value;
            if (_skill.Backward)
                _shiftDirection += MathF.PI;

            if (_skill.InvFrame)
                ((IHasModifiers) initializer).Modifiers.InvFrame = true;
        }

        public void Process(TimeSpan delta)
        {
            var dt = (float)delta.TotalSeconds;
            var dx = dt * _speed * MathF.Cos(_shiftDirection);
            var dy = dt * _speed * MathF.Sin(_shiftDirection);
            var newX = _position.X + dx;
            var newY = _position.Y + dy;

            if (!_arena.Bounds.DoesIntersect(newX, newY))
            {
                BeforeComplete();
                Completed?.Invoke(this);
                return;
            }

            _position.Set(newX, newY);

            if (_position.DistanceTo(_startPosition) >= _skill.MaxDistance)
            {
                BeforeComplete();
                Completed?.Invoke(this);
            }
        }

        private void BeforeComplete()
        {
            if (_skill.InvFrame)
                ((IHasModifiers) _initializer).Modifiers.InvFrame = false;
        }

        public void Stop()
        {
            BeforeComplete();
        }
    }
}
