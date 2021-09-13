using System;
using Kalavarda.Primitives.Abstract;

namespace Room.Core.Impl
{
    public class LevelMultiplier: ILevelMultiplier
    {
        public float GetRatio(ushort level)
        {
            return MathF.Pow(MathF.Sqrt(2), level - 1);
        }

        public float GetValue(float baseValue, ushort level)
        {
            return baseValue * GetRatio(level);
        }

        public long GetValue(int baseValue, ushort level)
        {
            return (long)MathF.Round(baseValue * GetRatio(level));
        }
    }
}
