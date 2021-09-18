using System;
using System.Collections.Generic;
using System.Linq;
using Kalavarda.Primitives.Abstract;
using Kalavarda.Primitives.Geometry;
using Kalavarda.Primitives.Process;
using Kalavarda.Primitives.Skills;
using Room.Core.Abstract;
using Room.Core.Models;

namespace Room.Core.Skills
{
    public class BombSkill: SkillBase
    {
        public TimeSpan WaitTime { get; }

        public int BombCount { get; }

        public float HpChange { get; }

        public BombSkill(float maxDistance, TimeSpan interval, TimeSpan waitTime, float hpChange, int count, ISkillProcessFactory processFactory)
            : base(maxDistance, interval, processFactory)
        {
            WaitTime = waitTime;
            BombCount = count;
            HpChange = hpChange;
        }

        public override string Name => "Бомба";
    }

    public class BombProcess: IProcess
    {
        public const string Blow = "Bomb_Blow";

        private readonly ISkilled _initializer;
        private readonly BombSkill _skill;
        private readonly Game _game;
        private readonly ISoundPlayer _soundPlayer;
        private readonly IHpChanger _hpChanger;
        private readonly ICollection<Bomb> _bombs = new List<Bomb>();
        private readonly DateTime _startTime;

        public event Action<IProcess> Completed;

        public BombProcess(ISkilled initializer, BombSkill skill, Game game, ISoundPlayer soundPlayer, IRandom random, IHpChanger hpChanger)
        {
            _initializer = initializer ?? throw new ArgumentNullException(nameof(initializer));
            _skill = skill ?? throw new ArgumentNullException(nameof(skill));
            _game = game ?? throw new ArgumentNullException(nameof(game));
            _soundPlayer = soundPlayer ?? throw new ArgumentNullException(nameof(soundPlayer));
            _hpChanger = hpChanger ?? throw new ArgumentNullException(nameof(hpChanger));

            var itemsOwner = ((IChildItemsOwnerExt)_initializer).ChildItemsContainer;
            foreach (var unused in Enumerable.Range(0, skill.BombCount))
            {
                var bomb = CreateBomb(random);
                itemsOwner.Add(bomb);
            }

            _startTime = DateTime.Now;
        }

        public void Process(TimeSpan delta)
        {
            if (_initializer is ICreature cr)
                if (cr.IsDead)
                {
                    BeforeComplete();
                    Completed?.Invoke(this);
                    return;
                }

            foreach (var bomb in _bombs.ToArray())
                if (bomb.Bounds.DoesIntersect(_game.Hero.Bounds))
                {
                    var childItemsOwner = (IChildItemsOwnerExt) _initializer;
                    childItemsOwner.ChildItemsContainer.Remove(bomb);
                    _bombs.Remove(bomb);

                    if (_bombs.Count == 0)
                    {
                        Stop();
                        return;
                    }
                }

            if (DateTime.Now - _startTime > _skill.WaitTime)
            {
                _soundPlayer.Play(Blow);
                foreach (var b in _game.GetAllBounds())
                    if (b is ICreatureExt creature)
                        if (creature != _initializer)
                            foreach (var bomb in _bombs)
                                if (b.Bounds.Position.DistanceTo(bomb.Bounds.Position) <= _skill.MaxDistance)
                                    _hpChanger.ApplyChange(creature, _skill.HpChange, _initializer, _skill);
                Stop();
            }
        }

        private Bomb CreateBomb(IRandom random)
        {
            var bomb = new Bomb(((IChildItemsOwner)_initializer).ChildItemsContainer);
            var x = (random.Float() - 0.5f) * _game.Arena.Bounds.Width;
            var y = (random.Float() - 0.5f) * _game.Arena.Bounds.Height;
            bomb.Bounds.Position.Set(x, y);
            _bombs.Add(bomb);
            return bomb;
        }

        public void Stop()
        {
            BeforeComplete();
            Completed?.Invoke(this);
        }

        private void BeforeComplete()
        {
            var childItemsOwner = (IChildItemsOwnerExt)_initializer;
            foreach (var bomb in _bombs)
                childItemsOwner.ChildItemsContainer.Remove(bomb);
        }
    }

    public class Bomb : IHasBounds, IPhysicalObject, IChildItem
    {
        public BoundsF Bounds { get; } = new RoundBounds(new PointF(), 0.5f);

        public float Speed => 0;

        public AngleF Direction => new AngleF();

        public IChildItemsContainer Container { get; }

        public Bomb(IChildItemsContainer itemsContainer)
        {
            Container = itemsContainer ?? throw new ArgumentNullException(nameof(itemsContainer));
        }
    }
}
