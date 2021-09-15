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
        public Game Game { get; set; }

        private readonly ISoundPlayer _soundPlayer;
        private readonly IHpChanger _hpChanger;

        public HeroSkillProcessFactory(ISoundPlayer soundPlayer, IHpChanger hpChanger)
        {
            _soundPlayer = soundPlayer;
            _hpChanger = hpChanger ?? throw new ArgumentNullException(nameof(hpChanger));
        }

        public IProcess Create(ISkilled initializer, ISkill skill)
        {
            if (skill is FireballSkill fireball)
            {
                var dx = skill.MaxDistance * MathF.Cos(Game.Hero.LookDirection.Value);
                var dy = skill.MaxDistance * MathF.Sin(Game.Hero.LookDirection.Value);
                var target = new RoundBounds(new PointF(Game.Hero.Position.X + dx, Game.Hero.Position.Y + dy), 0);

                return new FireballProcess((IHasBounds)initializer, fireball, target, Game, _soundPlayer, _hpChanger);
            }

            if (skill is TeleportSkill teleport)
                return new TeleportProcess(initializer, teleport, Game);

            throw new NotImplementedException();
        }
    }
}
