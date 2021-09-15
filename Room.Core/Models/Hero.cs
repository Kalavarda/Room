using System;
using System.Collections.Generic;
using System.Linq;
using Kalavarda.Primitives;
using Kalavarda.Primitives.Abstract;
using Kalavarda.Primitives.Geometry;
using Kalavarda.Primitives.Skills;
using Room.Core.Abstract;

namespace Room.Core.Models
{
    public class Hero: IHasPosition, IHasBounds, IPhysicalObject, ICreatureExt, ISkilled, IChildItemsOwner, IChildItemsOwnerExt, ILooking, IHasModifiers, IHasLevel
    {
        public const ushort MaxLevel = 50;

        public const string SkillKey_Fireball = "Fireball_Simple";
        public const string SkillKey_Teleport_Forward = "Teleport_Forward";
        public const string SkillKey_Teleport_Backward = "Teleport_Backward";
        public const string SkillKey_Healing = "Healing_Simple";
        public const string SkillKey_Use_ = "Use_";

        private readonly ISkill[] _skills;
        private ushort _level = 1;

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

        public ushort Level
        {
            get => _level;
            set
            {
                if (_level == value)
                    return;

                _level = value;
                LevelChanged?.Invoke();
            }
        }

        public event Action LevelChanged;

        public bool IsAlive => !IsDead;
        
        public bool IsDead { get; private set; }

        public event Action<ICreature> Died;

        public IModifiers Modifiers => Equipment.Modifiers;

        public void ChangeHP(float value, ISkilled initializer, ISkill skill)
        {
            var oldValue = HP.Value;
            HP.Value += value;
            HpChanged?.Invoke(new HpChange(this, HP.Value - oldValue, initializer, skill));
        }

        public event Action<HpChange> HpChanged;

        public IGameItemsContainerExt Bag { get; } = new GameItemsContainer();

        public HeroEquipment Equipment { get; } = new HeroEquipment();

        public IReadOnlyCollection<ISkill> Skills => _skills;
        
        public IChildItemsContainerExt ChildItemsContainer { get; } = new ChildItemsContainer();
        
        IChildItemsContainer IChildItemsOwner.ChildItemsContainer => ChildItemsContainer;

        public Hero(ISkillsFactory skillsFactory)
        {
            HP.SetMax();
            HP.ValueMin += hp =>
            {
                IsDead = true;
                Died?.Invoke(this);
            };

            _skills = skillsFactory.Create(this).ToArray();

            //Bag.TryChangeCount(GameItemTypes.SmallHealthPotion, 3);
            //Bag.TryChangeCount(EquipmentItem.OldNecklace, 1);
            //Bag.TryChangeCount(EquipmentItem.OldBelt, 1);
        }

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
