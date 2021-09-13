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
    public class Hero: IHasPosition, IHasBounds, IPhysicalObject, ICreatureExt, ISkilled, IChildItemsOwner, IChildItemsOwnerExt, ILooking, IHasModifiers, IHasLevel
    {
        public const string SkillKey_Fireball = "Fireball_Simple";
        public const string SkillKey_Teleport_Forward = "Teleport_Forward";
        public const string SkillKey_Teleport_Backward = "Teleport_Backward";
        public const string SkillKey_Healing = "Healing_Simple";
        public const string SkillKey_Use_ = "Use_";

        private readonly ISkill[] _skills;

        public PointF Position => Bounds.Position;

        public AngleF LookDirection { get; } = new AngleF();

        public AngleF MoveDirection { get; } = new AngleF();

        public RangeF MoveSpeed { get; } = new RangeF { Max = 2 * 5000f / 3600 };

        public SizeF Size => Bounds.Size;
        
        public BoundsF Bounds { get; } = new RoundBounds(new PointF(), 0.6f);

        public float Speed => MoveSpeed.Value;

        public AngleF Direction => MoveDirection;
        
        public RangeF HP { get; } = new RangeF { Max = 100 };

        public RangeF XP { get; } = new RangeF();

        public ushort Level { get; set; }

        public bool IsAlive => !IsDead;
        
        public bool IsDead { get; private set; }

        public event Action<ICreature> Died;

        public Modifiers Modifiers { get; } = new Modifiers();

        public void ChangeHP(float value, ISkilled initializer, ISkill skill)
        {
            if (!Modifiers.InvFrame) // TODO: unit-test
            {
                var oldValue = HP.Value;
                HP.Value += value;
                HpChanged?.Invoke(new HpChange(this, HP.Value - oldValue, initializer, skill));
            }
        }

        public event Action<HpChange> HpChanged;

        public IGameItemsContainerExt ItemsContainer { get; } = new GameItemsContainer();

        public Hero(ISkillProcessFactory skillProcessFactory)
        {
            HP.SetMax();
            HP.ValueMin += hp =>
            {
                IsDead = true;
                Died?.Invoke(this);
            };

            _skills = new ISkill[]
            {
                new FireballSkill(TimeSpan.FromSeconds(2), 3, 4, -25, skillProcessFactory) { Key = SkillKey_Fireball },
                new TeleportSkill(4, TimeSpan.FromSeconds(20), TimeSpan.FromSeconds(0.2), false, false, skillProcessFactory) { Key = SkillKey_Teleport_Forward },
                new TeleportSkill(1, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(0.2), true, true, skillProcessFactory) { Key = SkillKey_Teleport_Backward },
                new HealSkill(10, TimeSpan.FromSeconds(30)) { Key = SkillKey_Healing },
                new UseItemSkill(this, GameItemTypeTypes.SmallHealthPotion)
            };
        }

        public IReadOnlyCollection<ISkill> Skills => _skills;
        
        public IChildItemsContainerExt ChildItemsContainer { get; } = new ChildItemsContainer();
        
        IChildItemsContainer IChildItemsOwner.ChildItemsContainer => ChildItemsContainer;

        /// <summary>
        /// Воскресить
        /// </summary>
        public void Ressurect()
        {
            HP.SetMax();
            IsDead = false;
        }
    }
}
