using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Kalavarda.Primitives.Skills;
using Kalavarda.Primitives.WPF.Skills;
using Room.Core.Models;

namespace Room
{
    public class HeroSkillBinds: ISkillBinds
    {
        private readonly Hero _hero;

        public HeroSkillBinds(Hero hero)
        {
            _hero = hero ?? throw new ArgumentNullException(nameof(hero));
        }

        public IReadOnlyCollection<SkillBind> SkillBinds { get; } = new []
        {
            new SkillBind(Hero.SkillKey_1, Key.D1),
            new SkillBind(Hero.SkillKey_2, Key.D5),
            new SkillBind(Hero.SkillKey_2, null, MouseButton.Middle),
            new SkillBind(Hero.SkillKey_3, Key.C)
        };

        public ISkill GetSkill(string key)
        {
            switch (key)
            {
                case Hero.SkillKey_1:
                    return _hero.Skills.First();

                case Hero.SkillKey_2:
                    return _hero.Skills.Skip(1).First();

                case Hero.SkillKey_3:
                    return _hero.Skills.Skip(2).First();

                default:
                    throw new NotImplementedException();
            }
        }
    }
}
