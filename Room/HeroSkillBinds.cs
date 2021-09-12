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
            new SkillBind(Hero.SkillKey_Fireball, Key.D1),
            new SkillBind(Hero.SkillKey_Fireball, null, MouseButton.Left),
            new SkillBind(Hero.SkillKey_Teleport_Forward, Key.D5),
            new SkillBind(Hero.SkillKey_Teleport_Forward, null, MouseButton.Middle),
            new SkillBind(Hero.SkillKey_Teleport_Backward, Key.C),
            new SkillBind(Hero.SkillKey_Healing, Key.D2)
        };

        public ISkill GetSkill(string key)
        {
            return (ISkill)_hero.Skills.OfType<IHasKey>().First(sk => sk.Key == key);
        }

        public SkillBind GetBind(string key)
        {
            return SkillBinds.FirstOrDefault(sb => sb.SkillKey == key);
        }
    }
}
