using System;
using Kalavarda.Primitives.Abstract;
using Kalavarda.Primitives.Process;
using Kalavarda.Primitives.Skills;
using Room.Core.Models;
using Room.Core.Skills;

namespace Room.Core.Factories
{
    public class BossSkillProcessFactory : ISkillProcessFactory
    {
        private readonly Game _game;

        public BossSkillProcessFactory(Game game)
        {
            _game = game ?? throw new ArgumentNullException(nameof(game));
        }

        public IProcess Create(ISkilled initializer, ISkill skill)
        {
            if (skill is FireballSkill)
                return new FireballProcess((IHasBounds)initializer, skill, _game.Hero, _game);

            if (skill is RoundAreaSkill rAreaSkill)
                return new RoundAreaProcess(initializer, rAreaSkill, _game);

            throw new NotImplementedException();
        }
    }
}
