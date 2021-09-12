using System;
using Kalavarda.Primitives.Abstract;
using Kalavarda.Primitives.Skills;
using Room.Core.Abstract;
using Room.Core.Models;

namespace Room.Core.Factories
{
    public class ArenaFactory : IArenaFactory
    {
        private readonly ISkillProcessFactory _bossSkillProcessFactory;
        private readonly ILevelMultiplier _levelMultiplier;

        public ArenaFactory(ISkillProcessFactory bossSkillProcessFactory, ILevelMultiplier levelMultiplier)
        {
            _bossSkillProcessFactory = bossSkillProcessFactory ?? throw new ArgumentNullException(nameof(bossSkillProcessFactory));
            _levelMultiplier = levelMultiplier ?? throw new ArgumentNullException(nameof(levelMultiplier));
        }

        public Arena Create(ushort level)
        {
            switch (level)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                    var boss = new Boss(level, _bossSkillProcessFactory);
                    boss.HP.Max = _levelMultiplier.GetValue(1000, level);
                    boss.HP.SetMax();

                    var arena = new Arena(boss);
                    arena.Bounds.Size.Width = 20;
                    arena.Bounds.Size.Height = 20;
                    return arena;

                default:
                    throw new NotImplementedException();
            }
        }
    }
}
