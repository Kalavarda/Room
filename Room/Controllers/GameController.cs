using System;
using System.Windows.Threading;
using Kalavarda.Primitives.Abstract;
using Kalavarda.Primitives.Process;
using Room.Core.Abstract;
using Room.Core.Models;
using Room.Processes;
using Room.Windows;

namespace Room.Controllers
{
    public class GameController: IDisposable
    {
        private readonly TimeSpan _waitTime = TimeSpan.FromSeconds(3);

        private readonly Game _game;
        private readonly IAwardsSource _awardsSource;
        private readonly IFinesSource _finesSource;
        private readonly IProcessor _processor;
        private readonly ILevelMultiplier _levelMultiplier;
        private BossProcess _bossProcess;
        private readonly DispatcherTimer _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(0.5) };
        private DateTime _arenaCreationTime;

        public IGameWindow GameWindow { get; set; }

        public GameController(Game game, IAwardsSource awardsSource, IFinesSource finesSource, IProcessor processor, ILevelMultiplier levelMultiplier)
        {
            _game = game ?? throw new ArgumentNullException(nameof(game));
            _awardsSource = awardsSource ?? throw new ArgumentNullException(nameof(awardsSource));
            _finesSource = finesSource ?? throw new ArgumentNullException(nameof(finesSource));
            _processor = processor ?? throw new ArgumentNullException(nameof(processor));
            _levelMultiplier = levelMultiplier ?? throw new ArgumentNullException(nameof(levelMultiplier));

            SetXP();

            _game.Hero.ItemsContainer.Changed += ItemsContainer_Changed;
            _game.Hero.Died += Hero_Died;
            
            _game.ArenaChanged += _game_ArenaChanged;
            _timer.Tick += _timer_Tick;
        }

        private void ItemsContainer_Changed(IGameItemType type, long count)
        {
            if (type == GameItemTypes.XP)
            {
                var overXP = _game.Hero.XP.Value + count - _game.Hero.XP.Max;
                _game.Hero.XP.Value += count;
                if (_game.Hero.XP.IsMax)
                {
                    LevelUp();
                    _game.Hero.XP.Value += overXP;
                }
            }
        }

        private void LevelUp()
        {
            if (_game.Hero.Level >= Hero.MaxLevel)
                return;

            _game.Hero.Level++;
            SetXP();
        }

        private void SetXP()
        {
            _game.Hero.XP.Max = _levelMultiplier.GetValue(1000, _game.Hero.Level);
            _game.Hero.XP.SetMin();
        }

        private void _game_ArenaChanged(Game game, Arena oldArena, Arena newArena)
        {
            if (oldArena != null)
                oldArena.Boss.Died -= Boss_Died;

            _bossProcess?.Stop();
            _bossProcess = null;

            if (newArena != null)
            {
                newArena.Boss.Modifiers.InvFrame = true;
                newArena.Boss.Died += Boss_Died;
                _arenaCreationTime = DateTime.Now;
                _timer.Start();
            }
        }

        private void _timer_Tick(object sender, EventArgs e)
        {
            if (DateTime.Now - _arenaCreationTime > _waitTime)
            {
                _timer.Stop();
                _bossProcess = new BossProcess(_game.Arena.Boss, _game, _processor);
                _processor.Add(_bossProcess);
                _game.Arena.Boss.Modifiers.InvFrame = false;
            }

            GameWindow.ShowGetReady(_arenaCreationTime + _waitTime - DateTime.Now);
        }

        private void Boss_Died(ICreature boss)
        {
            var awards = _awardsSource.GetAwards((BossBase)boss);
            foreach (var pair in awards)
                _game.Hero.ItemsContainer.TryChangeCount(pair.Key, pair.Value);

            GameWindow.ShowInformation("Победа!", () =>
            {
                GameWindow.Close();
            });
        }

        private void Hero_Died(ICreature hero)
        {
            _game.Hero.MoveSpeed.Value = 0;
            var fines = _finesSource.Fine();
            foreach (var pair in fines)
                _game.Hero.ItemsContainer.TryChangeCount(pair.Key, pair.Value);

            GameWindow.ShowWarning("Не фортануло...", () =>
            {
                GameWindow.Close();
            });
        }

        public void Dispose()
        {
            _timer.Tick -= _timer_Tick;
            _timer.Stop();

            _game.Hero.Died -= Hero_Died;
            _game.Hero.ItemsContainer.Changed -= ItemsContainer_Changed;

            _game.ArenaChanged -= _game_ArenaChanged;
        }
    }
}
