using System;
using Kalavarda.Primitives.Abstract;
using Kalavarda.Primitives.Geometry;
using Kalavarda.Primitives.Process;
using Kalavarda.Primitives.Skills;
using Room.Core.Abstract;
using Room.Core.Models;

namespace Room.Core.Skills
{
    public class FireballSkill: SkillBase, IHasKey
    {
        public override string Name => "Сгусток огня";

        public float Speed { get; }

        public float HpChange { get; }

        public FireballSkill(TimeSpan interval, float speed, float maxDistance, float hpChange, ISkillProcessFactory processFactory)
            :base(maxDistance, interval, processFactory)
        {
            Speed = speed;
            HpChange = hpChange;
        }

        public string Key { get; set; }
    }

    public class FireballProcess : IProcess
    {
        public const string FireballCreated = "Fireball_Created";

        private readonly IHasBounds _initializer;
        private readonly FireballSkill _skill;
        private readonly IHasPosition _target;
        private readonly Game _game;
        private readonly ISoundPlayer _soundPlayer;
        private readonly PointF _startPos;

        public event Action<IProcess> Completed;

        public Fireball Fireball { get; }

        public FireballProcess(IHasBounds initializer, FireballSkill skill, IHasPosition target, Game game, ISoundPlayer soundPlayer)
        {
            _initializer = initializer ?? throw new ArgumentNullException(nameof(initializer));
            _skill = skill ?? throw new ArgumentNullException(nameof(skill));
            _target = target ?? throw new ArgumentNullException(nameof(target));
            _game = game ?? throw new ArgumentNullException(nameof(game));
            _soundPlayer = soundPlayer ?? throw new ArgumentNullException(nameof(soundPlayer));

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
            var fireball = new Fireball(container, _skill.Speed, angle);
            fireball.Bounds.Position.Set(_initializer.Bounds.Position);

            _soundPlayer.Play(FireballCreated);

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
                            creatureExt.ChangeHP(_skill.HpChange, (ISkilled)_initializer, _skill);
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
