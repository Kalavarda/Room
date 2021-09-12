using System;
using Kalavarda.Primitives.Abstract;
using Kalavarda.Primitives.Geometry;
using Kalavarda.Primitives.Process;
using Kalavarda.Primitives.Skills;
using Room.Core.Abstract;
using Room.Core.Models;

namespace Room.Core.Skills
{
    public class RoundAreaSkill: SkillBase
    {
        public override string Name => "Круг";

        /// <summary>
        /// Через сколько бахнет
        /// </summary>
        public TimeSpan WaitTime { get; }

        public float HpChange { get; }

        public RoundAreaSkill(float maxDistance, TimeSpan interval, TimeSpan waitTime, float hpChange, ISkillProcessFactory processFactory)
            :base(maxDistance, interval, processFactory)
        {
            WaitTime = waitTime;
            HpChange = hpChange;
        }
    }

    public class RoundAreaProcess: IProcess
    {
        public const string Blow = "Area_Blow";

        private readonly ISkilled _initializer;
        private readonly RoundAreaSkill _skill;
        private readonly Game _game;
        private readonly ISoundPlayer _soundPlayer;
        private readonly AreaBase _area;
        private readonly DateTime _startTime = DateTime.Now;

        public event Action<IProcess> Completed;

        public RoundAreaProcess(ISkilled initializer, RoundAreaSkill skill, Game game, ISoundPlayer soundPlayer)
        {
            _initializer = initializer ?? throw new ArgumentNullException(nameof(initializer));
            _skill = skill ?? throw new ArgumentNullException(nameof(skill));
            _game = game ?? throw new ArgumentNullException(nameof(game));
            _soundPlayer = soundPlayer ?? throw new ArgumentNullException(nameof(soundPlayer));

            var childItemsOwner = (IChildItemsOwnerExt)initializer;
            var center = ((IHasBounds)_initializer).Bounds.Position;
            _area = new RoundArea(new RoundBounds(center, skill.MaxDistance), childItemsOwner.ChildItemsContainer)
            {
                FullRemain = _skill.WaitTime,
                Remain = _skill.WaitTime
            };
            childItemsOwner.ChildItemsContainer.Add(_area);
        }

        public void Process(TimeSpan delta)
        {
            if (DateTime.Now - _startTime > _skill.WaitTime)
            {
                _soundPlayer.Play(Blow);
                foreach (var b in _game.GetAllBounds())
                    if (_area.Bounds.DoesIntersect(b.Bounds))
                        if (b is ICreatureExt creature)
                            if (creature != _initializer)
                                creature.ChangeHP(_skill.HpChange, _initializer, _skill);
                BeforeComplete();
                Completed?.Invoke(this);
            }

            _area.Remain = _startTime + _skill.WaitTime - DateTime.Now;
        }

        public void Stop()
        {
            BeforeComplete();
        }

        private void BeforeComplete()
        {
            var childItemsOwner = (IChildItemsOwnerExt) _initializer;
            childItemsOwner.ChildItemsContainer.Remove(_area);
        }
    }

    public abstract class AreaBase : IChildItem, IHasBounds
    {
        private TimeSpan _remain;

        public BoundsF Bounds { get; }

        public IChildItemsContainer Container { get; }

        public TimeSpan Remain
        {
            get => _remain;
            set
            {
                if (_remain == value)
                    return;

                _remain = value;

                RemainChanged?.Invoke(this);
            }
        }

        public TimeSpan FullRemain { get; set; }

        public event Action<AreaBase> RemainChanged;

        protected AreaBase(RoundBounds bounds, IChildItemsContainer container)
        {
            Bounds = bounds ?? throw new ArgumentNullException(nameof(bounds));
            Container = container ?? throw new ArgumentNullException(nameof(container));
        }
    }

    public class RoundArea: AreaBase
    {
        public RoundArea(RoundBounds bounds, IChildItemsContainer container): base(bounds, container)
        {
        }
    }
}
