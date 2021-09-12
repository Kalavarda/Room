using System;
using System.Collections.Generic;
using Kalavarda.Primitives;
using Kalavarda.Primitives.Abstract;
using Kalavarda.Primitives.Geometry;
using Kalavarda.Primitives.Skills;
using Room.Core.Abstract;
using Room.Core.Skills;

namespace Room.Core.Models
{
    public class Boss : BossBase
    {
        private readonly ISkill[] _skills;

        public override IReadOnlyCollection<ISkill> Skills => _skills;

        public Boss(ISkillProcessFactory skillProcessFactory): base(100)
        {
            _skills = new ISkill[]
            {
                new FireballSkill(TimeSpan.FromSeconds(1), 4, 15, -20, skillProcessFactory),
                new RoundAreaSkill(5, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(2), -10, skillProcessFactory),
                new RoundAreaSkill(15, TimeSpan.FromSeconds(15), TimeSpan.FromSeconds(1), -50, skillProcessFactory)
            };
        }
    }
}
