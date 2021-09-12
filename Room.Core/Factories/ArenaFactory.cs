using System;
using Kalavarda.Primitives.Skills;
using Room.Core.Abstract;
using Room.Core.Models;

namespace Room.Core.Factories
{
    public class ArenaFactory : IArenaFactory
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
                    arena.Bounds.Size.Width = 20;
                    arena.Bounds.Size.Height = 20;
                    return arena;

                default:
                    throw new NotImplementedException();
            }
        }
    }
}
