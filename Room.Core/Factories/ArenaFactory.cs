using System;
using Kalavarda.Primitives.Skills;
using Room.Core.Models;

namespace Room.Core.Factories
{
    public class ArenaFactory
    {
        private readonly ISkillProcessFactory _bossSkillProcessFactory;

        public ArenaFactory(ISkillProcessFactory bossSkillProcessFactory)
        {
            _bossSkillProcessFactory = bossSkillProcessFactory ?? throw new ArgumentNullException(nameof(bossSkillProcessFactory));
        }

        public Arena Create(int level)
        {
            switch (level)
            {
                case 1:
                    var boss = new Boss(_bossSkillProcessFactory);
                    boss.HP.Max = 100;
                    boss.HP.SetMax();

                    var arena = new Arena(boss);
                    arena.Size.Width = 20;
                    arena.Size.Height = 20;
                    return arena;

                default:
                    throw new NotImplementedException();
            }
        }
    }
}
