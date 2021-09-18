using System;
using System.Collections.Generic;
using Kalavarda.Primitives.Skills;
using Room.Core.Models;
using Room.Core.Skills;

namespace Room.Core.Factories
{
    public class BossSkillsFactory: ISkillsFactory
    {
        private readonly ISkillProcessFactory _processFactory;

        public BossSkillsFactory(ISkillProcessFactory processFactory)
        {
            _processFactory = processFactory ?? throw new ArgumentNullException(nameof(processFactory));
        }

        public IReadOnlyCollection<ISkill> Create(ISkilled skilled)
        {
            var list = new List<ISkill>();
            list.AddRange(new ISkill[]
            {
                new FireballSkill(TimeSpan.FromSeconds(1), 4, 15, -20, _processFactory),
                new RoundAreaSkill(5, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(2), -10, 1, _processFactory),
                new RoundAreaSkill(15, TimeSpan.FromSeconds(20), TimeSpan.FromSeconds(1), -50, 1, _processFactory),
                new RoundAreaSkill(30, TimeSpan.FromSeconds(15), TimeSpan.FromSeconds(2), -25, 20, _processFactory)
            });
            
            if (skilled is BossBase boss)
                if (boss.Level > 1)
                    list.Add(new BombSkill(100, TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(15), -100, 2, _processFactory));

            return list;
        }
    }
}
