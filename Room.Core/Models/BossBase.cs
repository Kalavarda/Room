using System;
using System.Collections.Generic;
using Kalavarda.Primitives;
using Kalavarda.Primitives.Abstract;
using Kalavarda.Primitives.Geometry;
using Kalavarda.Primitives.Skills;
using Room.Core.Abstract;

namespace Room.Core.Models
{
    public abstract class BossBase : IHasBounds, IHasPosition, ISkilled, IChildItemsOwner, IChildItemsOwnerExt, ICreatureExt, IPhysicalObject, IHasModifiers, IHasLevel
    {
        public BoundsF Bounds { get; } = new RoundBounds(new PointF(), 1.5f);
        
        public float Speed => 0;
        
        public AngleF Direction { get; } = new AngleF();
        
        public PointF Position => Bounds.Position;
        
        public abstract IReadOnlyCollection<ISkill> Skills { get; }
        
        public RangeF HP { get; } = new RangeF { Max = 100 };
        
        public bool IsAlive => !IsDead;
        
        public bool IsDead { get; private set; }
        
        public IModifiers Modifiers { get; } = new Modifiers();
        
        public IChildItemsContainerExt ChildItemsContainer { get; } = new ChildItemsContainer();
        
        IChildItemsContainer IChildItemsOwner.ChildItemsContainer => ChildItemsContainer;

        public event Action<ICreature> Died;

        public event Action<HpChange> HpChanged;

        protected BossBase(ushort level)
        {
            Level = level;
            HP.SetMax();
            HP.ValueMin += HP_ValueMin;

            Modifiers.Attack = level;
            Modifiers.Defence = level;
        }

        public void ChangeHP(float value, ISkilled initializer, ISkill skill)
        {
            var oldValue = HP.Value;
            HP.Value += value;
            HpChanged?.Invoke(new HpChange(this, HP.Value - oldValue, initializer, skill));
        }

        private void HP_ValueMin(RangeF hp)
        {
            IsDead = true;
            Died?.Invoke(this);
        }

        public ushort Level { get; }
    }
}