using System;
using System.Runtime.CompilerServices;
using Kalavarda.Primitives.Abstract;
using Kalavarda.Primitives.Skills;
using Room.Core.Abstract;

[assembly:InternalsVisibleTo("Room.Core.Tests")]

namespace Room.Core.Impl
{
    public class HpChanger : IHpChanger
    {
        private readonly ILevelMultiplier _levelMultiplier;

        public HpChanger(ILevelMultiplier levelMultiplier)
        {
            _levelMultiplier = levelMultiplier ?? throw new ArgumentNullException(nameof(levelMultiplier));
        }

        public void ApplyChange(ICreatureExt targetCreature, float hpChange, ISkilled initializer, ISkill skill)
        {
            if (targetCreature == null) throw new ArgumentNullException(nameof(targetCreature));

            hpChange = CalculateHpChange(targetCreature, hpChange, initializer);

            if (Math.Abs(hpChange) < 0.01)
                return;

            targetCreature.ChangeHP(hpChange, initializer, skill);
        }

        internal float CalculateHpChange(ICreatureExt targetCreature, float hpChange, ISkilled initializer)
        {
            if (targetCreature is IHasModifiers hasModifiers)
            {
                var defModifiers = hasModifiers.Modifiers;

                if (defModifiers.InvFrame)
                    return 0;

                var defRatio = _levelMultiplier.GetRatio(defModifiers.Defence);
                hpChange /= defRatio;
            }

            if (initializer is IHasModifiers hasModif)
            {
                var attackModifiers = hasModif.Modifiers;

                var attackRatio = _levelMultiplier.GetRatio(attackModifiers.Attack);
                hpChange *= attackRatio;
            }

            return hpChange;
        }
    }
}
