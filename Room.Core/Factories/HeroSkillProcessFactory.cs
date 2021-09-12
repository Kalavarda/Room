using System;
using Kalavarda.Primitives.Abstract;
using Kalavarda.Primitives.Geometry;
using Kalavarda.Primitives.Process;
using Kalavarda.Primitives.Skills;
using Room.Core.Abstract;
using Room.Core.Models;
using Room.Core.Skills;

namespace Room.Core.Factories
{
    public class HeroSkillProcessFactory : ISkillProcessFactory
    {
        private readonly Game _game;
        private readonly ISoundPlayer _soundPlayer;

        public HeroSkillProcessFactory(Game game, ISoundPlayer soundPlayer)
        {
            _game = game ?? throw new ArgumentNullException(nameof(game));
            _soundPlayer = soundPlayer;
        }

        public IProcess Create(ISkilled initializer, ISkill skill)
        {
            if (skill is FireballSkill fireball)
            {
                var dx = skill.MaxDistance * MathF.Cos(_game.Hero.LookDirection.Value);
                var dy = skill.MaxDistance * MathF.Sin(_game.Hero.LookDirection.Value);
                var target = new RoundBounds(new PointF(_game.Hero.Position.X + dx, _game.Hero.Position.Y + dy), 0);

                return new FireballProcess((IHasBounds)initializer, fireball, target, _game, _soundPlayer);
            }

            if (skill is TeleportSkill teleport)
                return new TeleportProcess(initializer, teleport, _game);

            throw new NotImplementedException();
        }
    }
}
