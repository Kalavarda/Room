using System;
using Kalavarda.Primitives;
using Kalavarda.Primitives.Abstract;
using Kalavarda.Primitives.Geometry;
using Kalavarda.Primitives.Process;
using Kalavarda.Primitives.Skills;
using Room.Core.Models;

namespace Room.Core.Skills
{
    public class FireballSkill: ISkill
    {
        private readonly TimeLimiter _timeLimiter;

        public string Name => "Сгусток огня";

        public float MaxDistance { get; }

        public float Speed { get; }

        public ITimeLimiter TimeLimiter => _timeLimiter;

        public ISkillProcessFactory SkillProcessFactory { get; }

        public FireballSkill(TimeSpan interval, float speed, float maxDistance, ISkillProcessFactory processFactory)
        {
            MaxDistance = maxDistance;
            _timeLimiter = new TimeLimiter(interval);
            SkillProcessFactory = processFactory ?? throw new ArgumentNullException(nameof(processFactory));
            Speed = speed;
        }

        public IProcess Use(ISkilled initializer)
        {
            IProcess fireballProcess = null;
            _timeLimiter.Do(() =>
            {
                if (initializer is ICreature creature)
                    if (creature.IsDead)
                        return;

                fireballProcess = SkillProcessFactory.Create(initializer, this);
            });
            return fireballProcess;
        }
    }

    public class FireballProcess : IProcess
    {
        private readonly IHasBounds _initializer;
        private readonly ISkill _skill;
        private readonly IHasPosition _target;
        private readonly Game _game;
        private readonly PointF _startPos;

        public event Action<IProcess> Completed;

        public Fireball Fireball { get; }

        public FireballProcess(IHasBounds initializer, ISkill skill, IHasPosition target, Game game)
        {
            _initializer = initializer ?? throw new ArgumentNullException(nameof(initializer));
            _skill = skill ?? throw new ArgumentNullException(nameof(skill));
            _target = target ?? throw new ArgumentNullException(nameof(target));
            _game = game ?? throw new ArgumentNullException(nameof(game));

            Fireball = CreateFireball();
            var itemsOwner = ((IChildItemsOwnerExt)_initializer).ChildItemsContainer;
            itemsOwner.Add(Fireball);

            _startPos = Fireball.Bounds.Position.DeepClone();
        }

        private Fireball CreateFireball()
        {
            var dx = _target.Position.X - _initializer.Bounds.Position.X;
            var dy = _target.Position.Y - _initializer.Bounds.Position.Y;
            var angle = new AngleF { Value = MathF.Atan2(dy, dx) };

            var container = ((IChildItemsOwnerExt)_initializer).ChildItemsContainer;
            var fireball = new Fireball(container, ((FireballSkill)_skill).Speed, angle);
            fireball.Bounds.Position.Set(_initializer.Bounds.Position);
            return fireball;
        }

        public void Process(TimeSpan delta)
        {
            var distance = _startPos.DistanceTo(Fireball.Bounds);
            if (distance > _skill.MaxDistance)
            {
                BeforeComplete();
                Completed?.Invoke(this);
                return;
            }

            var dt = (float)delta.TotalSeconds;
            var x = Fireball.Bounds.Position.X + dt * Fireball.Speed * MathF.Cos(Fireball.Direction.Value);
            var y = Fireball.Bounds.Position.Y + dt * Fireball.Speed * MathF.Sin(Fireball.Direction.Value);
            Fireball.Bounds.Position.Set(x, y);

            foreach (var obj in _game.GetAllBounds())
                if (obj != _initializer)
                    if (obj.Bounds.DoesIntersect(Fireball.Bounds))
                    {
                        if (obj is ICreatureExt creatureExt)
                            creatureExt.ChangeHP(-10, (ISkilled)_initializer, _skill);
                        BeforeComplete();
                        Completed?.Invoke(this);
                        return;
                    }
        }

        private void BeforeComplete()
        {
            var itemsOwner = ((IChildItemsContainerExt) Fireball.Container);
            itemsOwner.Remove(Fireball);
        }

        public void Stop()
        {
            BeforeComplete();
        }
    }

    public class Fireball: IHasBounds, IPhysicalObject, IChildItem
    {
        public BoundsF Bounds { get; } = new RoundBounds(new PointF(), 0.1f);
        
        public float Speed { get; }
        
        public AngleF Direction { get; }

        public IChildItemsContainer Container { get; }

        public Fireball(IChildItemsContainer itemsContainer, float speed, AngleF direction)
        {
            Container = itemsContainer ?? throw new ArgumentNullException(nameof(itemsContainer));
            Speed = speed;
            Direction = direction;
        }
    }
}
