using System.Collections.Generic;
using System.Linq;
using Kalavarda.Primitives.Skills;

namespace Room.Core.Models
{
    public class Boss : BossBase
    {
        private readonly ISkill[] _skills;

        public override IReadOnlyCollection<ISkill> Skills => _skills;

        public Boss(ushort level, ISkillsFactory skillsFactory) : base(level)
        {
            _skills = skillsFactory.Create(this).ToArray();
        }
    }
}
