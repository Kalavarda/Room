using System;
using System.Collections.Generic;
using Kalavarda.Primitives;
using Kalavarda.Primitives.Abstract;
using Kalavarda.Primitives.Geometry;
using Kalavarda.Primitives.Skills;
using Room.Core.Abstract;
using Room.Core.Skills;

namespace Room.Core.Models
{
    public class Boss : IHasBounds, IHasPosition, ISkilled, IChildItemsOwner, IChildItemsOwnerExt, ICreatureExt, IPhysicalObject, IHasModifiers
    {
        private readonly ISkill[] _skills;

        public BoundsF Bounds { get; } = new RoundBounds(new PointF(), 1.5f);

        public float Speed => 0;

        public AngleF Direction { get; } = new AngleF();

        public PointF Position => Bounds.Position;
        
        public IReadOnlyCollection<ISkill> Skills => _skills;

        public Boss(ISkillProcessFactory skillProcessFactory)
        {
            _skills = new ISkill[]
            {
                new FireballSkill(TimeSpan.FromSeconds(1), 4, 15, skillProcessFactory),
                new RoundAreaSkill(5, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(2), skillProcessFactory),
                new RoundAreaSkill(15, TimeSpan.FromSeconds(15), TimeSpan.FromSeconds(1), skillProcessFactory)
            };

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

        public Modifiers Modifiers { get; } = new Modifiers();

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
}
