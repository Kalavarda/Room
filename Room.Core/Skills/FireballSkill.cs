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
        private readonly Hero _hero;
        private readonly TimeLimiter _timeLimiter = new TimeLimiter(TimeSpan.FromSeconds(1.5));

        public string Name => "Сгусток огня";

        public float MaxDistance => 20;

        public ITimeLimiter TimeLimiter => _timeLimiter;

        public FireballSkill(Hero hero)
        {
            _hero = hero ?? throw new ArgumentNullException(nameof(hero));
        }

        public IProcess Use(ISkilled initializer)
        {
            FireballProcess fireballProcess = null;
            _timeLimiter.Do(() =>
            {
                var initBounds = (IHasBounds) initializer;
                var dx = _hero.Position.X - initBounds.Bounds.Position.X;
                var dy = _hero.Position.Y - initBounds.Bounds.Position.Y;
                var angle = new AngleF { Value = MathF.Atan2(dy, dx) };

                var itemsOwner = (IChildItemsOwnerExt)initializer;
                var fireball = new Fireball(itemsOwner);
                itemsOwner.Add(fireball);
                fireballProcess = new FireballProcess(fireball, angle, 3);
                fireballProcess.Completed += FireballProcess_Completed;
            });
            return fireballProcess;
        }

        private void FireballProcess_Completed(IProcess p)
        {
            var fireball = ((FireballProcess)p).Fireball;
            var itemsOwner = (IChildItemsOwnerExt)fireball.Owner;
            itemsOwner.Remove(fireball);
        }
    }

    public class FireballProcess : IProcess
    {
        private readonly AngleF _angle;
        private readonly float _speed;
        private readonly DateTime _startTime = DateTime.Now;
        private readonly TimeSpan _duration = TimeSpan.FromSeconds(4);

        public event Action<IProcess> Completed;

        public Fireball Fireball { get; }

        public FireballProcess(Fireball fireball, AngleF angle, float speed)
        {
            _angle = angle ?? throw new ArgumentNullException(nameof(angle));
            _speed = speed;
            Fireball = fireball ?? throw new ArgumentNullException(nameof(fireball));
        }

        public void Process(TimeSpan delta)
        {
            if (DateTime.Now - _startTime > _duration)
            {
                Completed?.Invoke(this);
                return;
            }

            var dt = (float)delta.TotalSeconds;
            var x = Fireball.Bounds.Position.X + dt * _speed * MathF.Cos(_angle.Value);
            var y = Fireball.Bounds.Position.Y + dt * _speed * MathF.Sin(_angle.Value);
            Fireball.Bounds.Position.Set(x, y);
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }
    }

    public class Fireball: IHasBounds, IChildItem
    {
        public BoundsF Bounds { get; } = new RoundBounds(new PointF(), 0.1f);

        public IChildItemsOwner Owner { get; }

        public Fireball(IChildItemsOwner owner)
        {
            Owner = owner ?? throw new ArgumentNullException(nameof(owner));
        }
    }
}
