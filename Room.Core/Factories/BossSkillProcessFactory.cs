using System;
using Kalavarda.Primitives.Abstract;
using Kalavarda.Primitives.Process;
using Kalavarda.Primitives.Skills;
using Room.Core.Abstract;
using Room.Core.Models;
using Room.Core.Skills;

namespace Room.Core.Factories
{
    public class BossSkillProcessFactory : ISkillProcessFactory
    {
        private readonly Game _game;
        private readonly ISoundPlayer _soundPlayer;

        public BossSkillProcessFactory(Game game, ISoundPlayer soundPlayer)
        {
            _game = game ?? throw new ArgumentNullException(nameof(game));
            _soundPlayer = soundPlayer;
        }

        public IProcess Create(ISkilled initializer, ISkill skill)
        {
            if (skill is FireballSkill fireball)
                return new FireballProcess((IHasBounds)initializer, fireball, _game.Hero, _game, _soundPlayer);

            if (skill is RoundAreaSkill rAreaSkill)
                return new RoundAreaProcess(initializer, rAreaSkill, _game, _soundPlayer);

            throw new NotImplementedException();
        }
    }
}
