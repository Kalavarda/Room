using System;
using System.Collections.Generic;
using System.Linq;
using Kalavarda.Primitives.Abstract;
using Kalavarda.Primitives.Geometry;
using Kalavarda.Primitives.Process;
using Kalavarda.Primitives.Skills;
using Kalavarda.Primitives.Utils;
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

        public int Count { get; }

        public RoundAreaSkill(float maxDistance, TimeSpan interval, TimeSpan waitTime, float hpChange, int count, ISkillProcessFactory processFactory)
            :base(maxDistance, interval, processFactory, TimeSpan.FromSeconds(interval.TotalSeconds * RandomImpl.Instance.Double()))
        {
            WaitTime = waitTime;
            HpChange = hpChange;
            Count = count;
        }
    }

    public class RoundAreaProcess: IProcess
    {
        public const string Blow = "Area_Blow";

        private readonly ISkilled _initializer;
        private readonly RoundAreaSkill _skill;
        private readonly Game _game;
        private readonly ISoundPlayer _soundPlayer;
        private readonly IRandom _random;
        private readonly IHpChanger _hpChanger;
        private readonly ICollection<AreaBase> _areas = new List<AreaBase>();
        private readonly DateTime _startTime = DateTime.Now;

        public event Action<IProcess> Completed;

        public RoundAreaProcess(ISkilled initializer, RoundAreaSkill skill, Game game, ISoundPlayer soundPlayer, IRandom random, IHpChanger hpChanger)
        {
            _initializer = initializer ?? throw new ArgumentNullException(nameof(initializer));
            _skill = skill ?? throw new ArgumentNullException(nameof(skill));
            _game = game ?? throw new ArgumentNullException(nameof(game));
            _soundPlayer = soundPlayer ?? throw new ArgumentNullException(nameof(soundPlayer));
            _random = random ?? throw new ArgumentNullException(nameof(random));
            _hpChanger = hpChanger ?? throw new ArgumentNullException(nameof(hpChanger));

            CreateAreas(skill, (IChildItemsOwnerExt)initializer);
        }

        private void CreateAreas(RoundAreaSkill skill, IChildItemsOwnerExt childItemsOwner)
        {
            var w = _game.Arena.Bounds.Width / 2;
            var h = _game.Arena.Bounds.Height / 2;
            var maxR = MathF.Sqrt(w * w + h * h);

            var areas = new List<RoundArea>();
            for (var i = 0; i < _skill.Count; i++)
            {
                RoundArea area;
                if (_skill.Count > 1)
                {
                    RoundBounds bounds;
                    do
                    {
                        var r = maxR * _random.Float();
                        var a = 2 * MathF.PI * _random.Float();
                        var x = _game.Arena.Bounds.Position.X + r * MathF.Cos(a);
                        var y = _game.Arena.Bounds.Position.Y + r * MathF.Sin(a);
                        bounds = new RoundBounds(new PointF(x, y), skill.MaxDistance / skill.Count);
                    } while (areas.Select(a => a.Bounds).Any(b => b.DoesIntersect(bounds)));
                    area = CreateArea(childItemsOwner, bounds);
                    areas.Add(area);
                }
                else
                {
                    var center = ((IHasBounds) _initializer).Bounds.Position;
                    area = CreateArea(childItemsOwner, new RoundBounds(center, skill.MaxDistance / skill.Count));
                }

                _areas.Add(area);
            }
        }

        private RoundArea CreateArea(IChildItemsOwnerExt childItemsOwner, RoundBounds bounds)
        {
            var area = new RoundArea(bounds, childItemsOwner.ChildItemsContainer)
            {
                FullRemain = _skill.WaitTime,
                Remain = _skill.WaitTime
            };
            childItemsOwner.ChildItemsContainer.Add(area);
            return area;
        }

        public void Process(TimeSpan delta)
        {
            if (_initializer is ICreature cr)
                if (cr.IsDead)
                {
                    Stop();
                    return;
                }

            if (DateTime.Now - _startTime > _skill.WaitTime)
            {
                _soundPlayer.Play(Blow);
                foreach (var b in _game.GetAllBounds())
                    foreach (var area in _areas)
                        if (area.Bounds.DoesIntersect(b.Bounds))
                            if (b is ICreatureExt creature)
                                if (creature != _initializer)
                                    _hpChanger.ApplyChange(creature, _skill.HpChange, _initializer, _skill);
                Stop();
            }

            foreach (var area in _areas)
                area.Remain = _startTime + _skill.WaitTime - DateTime.Now;
        }

        public void Stop()
        {
            BeforeComplete();
            Completed?.Invoke(this);
        }

        private void BeforeComplete()
        {
            var childItemsOwner = (IChildItemsOwnerExt) _initializer;
            foreach (var area in _areas)
                childItemsOwner.ChildItemsContainer.Remove(area);
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
