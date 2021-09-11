using System;
using System.Collections.Generic;
using System.Linq;
using Kalavarda.Primitives;
using Kalavarda.Primitives.Abstract;
using Kalavarda.Primitives.Geometry;
using Kalavarda.Primitives.Process;
using Kalavarda.Primitives.Skills;
using Room.Core.Skills;

namespace Room.Core.Models
{
    public class Boss : IHasBounds, IHasPosition, ISkilled, IChildItemsOwner, IChildItemsOwnerExt, ICreatureExt, IPhysicalObject
    {
        private readonly ISkill[] _skills;

        public BoundsF Bounds { get; } = new RoundBounds(new PointF(), 1.5f);

        public float Speed => 0;

        public AngleF Direction { get; } = new AngleF();

        public PointF Position => Bounds.Position;
        
        public IReadOnlyCollection<ISkill> Skills => _skills;

        public Boss(Game game)
        {
            var fireball = new FireballSkill(TimeSpan.FromSeconds(1), 4, 15, new BossFireballProcessFactory(this, game));
            _skills = new [] { fireball };

            HP.SetMax();
            HP.ValueMin += HP_ValueMin;
        }

        private void HP_ValueMin(RangeF hp)
        {
            IsDead = true;
            Died?.Invoke(this);
        }

        public RangeF HP { get; } = new RangeF { Max = 1000 };

        public bool IsAlive => !IsDead;

        public bool IsDead { get; private set; }

        public event Action<ICreature> Died;

        public void ChangeHP(float value, ISkilled initializer, ISkill skill)
        {
            var oldValue = HP.Value;
            HP.Value += value;
            HpChanged?.Invoke(new HpChange(this, HP.Value - oldValue, initializer, skill));
        }

        public event Action<HpChange> HpChanged;

        public IChildItemsContainerExt ChildItemsContainer { get; } = new ChildItemsContainer();
        
        IChildItemsContainer IChildItemsOwner.ChildItemsContainer => ChildItemsContainer;
    }

    public class BossProcess: IProcess
    {
        private readonly Boss _boss;
        private readonly Game _game;
        private readonly IProcessor _processor;

        private bool wait = true;

        public event Action<IProcess> Completed;

        public BossProcess(Boss boss, Game game, IProcessor processor)
        {
            _boss = boss ?? throw new ArgumentNullException(nameof(boss));
            _game = game ?? throw new ArgumentNullException(nameof(game));
            _processor = processor ?? throw new ArgumentNullException(nameof(processor));
        }

        public void Process(TimeSpan delta)
        {
            var distance = _boss.Position.DistanceTo(_game.Hero.Position);

            // TODO: сделать нормально
            if (distance > 5)
                wait = false;
            if (wait)
                return;

            var skill = _boss.GetReadySkills()
                .Where(sk => sk.MaxDistance >= distance)
                .OrderByDescending(sk => sk.TimeLimiter.Interval)
                .FirstOrDefault();
            var skillProcess = skill?.Use(_boss);
            if (skillProcess != null)
                _processor.Add(skillProcess);
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}
