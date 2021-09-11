using System;
using System.Collections.Generic;
using System.Linq;
using Kalavarda.Primitives.Abstract;
using Kalavarda.Primitives.Geometry;
using Kalavarda.Primitives.Process;
using Kalavarda.Primitives.Skills;
using Room.Core.Skills;

namespace Room.Core.Models
{
    public class Boss : IHasBounds, IHasPosition, ISkilled, IChildItemsOwnerExt
    {
        private readonly ISkill[] _skills;

        private readonly ICollection<IChildItem> _childItems = new List<IChildItem>();

        public BoundsF Bounds { get; } = new RoundBounds(new PointF(), 1.5f);

        public PointF Position => Bounds.Position;
        
        public IReadOnlyCollection<ISkill> Skills => _skills;

        public Boss(Game game)
        {
            _skills = new [] {
                new FireballSkill(game.Hero)
            };
        }

        public void Add(IChildItem item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            lock(_childItems)
                _childItems.Add(item);
            ChildItemAdded?.Invoke(this, item);
        }

        public void Remove(IChildItem item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            lock (_childItems)
                _childItems.Remove(item);
            ChildItemRemoved?.Invoke(this, item);
        }

        public event Action<IChildItemsOwner, IChildItem> ChildItemAdded;
        public event Action<IChildItemsOwner, IChildItem> ChildItemRemoved;
    }

    public class BossProcess: IProcess
    {
        private readonly Boss _boss;
        private readonly IProcessor _processor;

        public event Action<IProcess> Completed;

        public BossProcess(Boss boss, IProcessor processor)
        {
            _boss = boss ?? throw new ArgumentNullException(nameof(boss));
            _processor = processor ?? throw new ArgumentNullException(nameof(processor));
        }

        public void Process(TimeSpan delta)
        {
            var skill = _boss.GetReadySkills()
                .OrderByDescending(sk => sk.TimeLimiter.Interval)
                .FirstOrDefault();
            var skillProcess = skill?.Use(_boss);
            if (skillProcess != null)
                _processor.Add(skillProcess);
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}
