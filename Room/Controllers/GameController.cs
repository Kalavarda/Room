using System;
using System.Windows.Threading;
using Kalavarda.Primitives.Process;
using Room.Core.Abstract;
using Room.Core.Models;
using Room.Processes;
using Room.Windows;

namespace Room.Controllers
{
    public class GameController
    {
        private readonly TimeSpan WaitTime = TimeSpan.FromSeconds(7);

        private readonly Game _game;
        private readonly IAwardsSource _awardsSource;
        private readonly IFinesSource _finesSource;
        private readonly IGameWindow _gameWindow;
        private readonly IArenaFactory _arenaFactory;
        private readonly IProcessor _processor;
        private BossProcess _bossProcess;
        private readonly DispatcherTimer _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(0.5) };
        private DateTime _arenaCreationTime;

        public GameController(Game game, IAwardsSource awardsSource, IFinesSource finesSource, IGameWindow gameWindow, IArenaFactory arenaFactory, IProcessor processor)
        {
            _game = game ?? throw new ArgumentNullException(nameof(game));
            _awardsSource = awardsSource ?? throw new ArgumentNullException(nameof(awardsSource));
            _finesSource = finesSource ?? throw new ArgumentNullException(nameof(finesSource));
            _gameWindow = gameWindow ?? throw new ArgumentNullException(nameof(gameWindow));
            _arenaFactory = arenaFactory ?? throw new ArgumentNullException(nameof(arenaFactory));
            _processor = processor ?? throw new ArgumentNullException(nameof(processor));

            _game.Hero.Died += Hero_Died;
            _game.ArenaChanged += _game_ArenaChanged;

            _timer.Tick += _timer_Tick;

            StartNewArena(1);
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
            if (DateTime.Now - _arenaCreationTime > WaitTime)
            {
                _timer.Stop();
                _bossProcess = new BossProcess(_game.Arena.Boss, _game, _processor);
                _processor.Add(_bossProcess);
                _game.Arena.Boss.Modifiers.InvFrame = false;
            }

            _gameWindow.ShowGetReady(_arenaCreationTime + WaitTime - DateTime.Now);
        }

        private void Boss_Died(Kalavarda.Primitives.Abstract.ICreature boss)
        {
            _game.Hero.MoveSpeed.Value = 0;
            _awardsSource.Award();
            _gameWindow.ShowInformation("Это успех!", () =>
            {
                StartNewArena(1);
            });
        }

        private void Hero_Died(Kalavarda.Primitives.Abstract.ICreature hero)
        {
            _game.Hero.MoveSpeed.Value = 0;
            _finesSource.Fine();
            _gameWindow.ShowWarning("Не фортануло...", () =>
            {
                StartNewArena(1);
            });
        }

        private void StartNewArena(int level)
        {
            _game.Hero.Ressurect();
            _game.Arena = _arenaFactory.Create(level);
            _game.Hero.Position.Set(-_game.Arena.Bounds.Width / 4, _game.Arena.Bounds.Height / 4);
        }
    }
}
