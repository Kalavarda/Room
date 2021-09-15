using Kalavarda.Primitives.Abstract;
using Kalavarda.Primitives.Skills;

namespace Room.Core.Abstract
{
    public interface IHpChanger
    {
        void ApplyChange(ICreatureExt targetCreature, float hpChange, ISkilled initializer, ISkill skill);
    }
}