using System;
using Kalavarda.Primitives.Abstract;
using Kalavarda.Primitives.Geometry;
using Kalavarda.Primitives.Process;
using Kalavarda.Primitives.Skills;
using Room.Core.Models;
using Room.Core.Skills;

namespace Room.Core.Factories
{
    public class HeroSkillProcessFactory : ISkillProcessFactory
    {
        private readonly Game _game;

        public HeroSkillProcessFactory(Game game)
        {
            _game = game ?? throw new ArgumentNullException(nameof(game));
        }

        public IProcess Create(ISkilled initializer, ISkill skill)
        {
            if (skill is FireballSkill)
            {
                var dx = skill.MaxDistance * MathF.Cos(_game.Hero.LookDirection.Value);
                var dy = skill.MaxDistance * MathF.Sin(_game.Hero.LookDirection.Value);
                var target = new RoundBounds(new PointF(_game.Hero.Position.X + dx, _game.Hero.Position.Y + dy), 0);

                return new FireballProcess((IHasBounds)initializer, skill, target, _game);
            }

            throw new NotImplementedException();
        }
    }
}
