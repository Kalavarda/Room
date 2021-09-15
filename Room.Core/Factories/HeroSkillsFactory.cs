using System;
using System.Collections.Generic;
using Kalavarda.Primitives.Skills;
using Room.Core.Abstract;
using Room.Core.Models;
using Room.Core.Skills;

namespace Room.Core.Factories
{
    public class HeroSkillsFactory: ISkillsFactory
    {
        private readonly ISkillProcessFactory _skillProcessFactory;
        private readonly IHpChanger _hpChanger;

        public HeroSkillsFactory(ISkillProcessFactory skillProcessFactory, IHpChanger hpChanger)
        {
            _skillProcessFactory = skillProcessFactory ?? throw new ArgumentNullException(nameof(skillProcessFactory));
            _hpChanger = hpChanger ?? throw new ArgumentNullException(nameof(hpChanger));
        }

        public IReadOnlyCollection<ISkill> Create(ISkilled skilled)
        {
            return new ISkill[]
            {
                new FireballSkill(TimeSpan.FromSeconds(2), 3, 4, -50, _skillProcessFactory) { Key = Hero.SkillKey_Fireball },
                new TeleportSkill(4, TimeSpan.FromSeconds(20), TimeSpan.FromSeconds(0.2), false, false, _skillProcessFactory) { Key = Hero.SkillKey_Teleport_Forward },
                new TeleportSkill(1, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(0.2), true, true, _skillProcessFactory) { Key = Hero.SkillKey_Teleport_Backward },
                new HealSkill(10, TimeSpan.FromSeconds(30), _hpChanger) { Key = Hero.SkillKey_Healing },
                new UseItemSkill((Hero)skilled, GameItemTypes.SmallHealthPotion, _hpChanger)
            };
        }
    }
}
