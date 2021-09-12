using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Kalavarda.Primitives.Controllers;
using Kalavarda.Primitives.Skills;
using Kalavarda.Primitives.WPF.Controllers;
using Room.Controllers;
using Room.Core.Models;

namespace Room.Controls
{
    public partial class GameControl
    {
        private ArenaControl _arenaControl;
        private HeroControl _heroControl;
        private BossControl _bossControl;

        private Game _game;
        private AppContext _appContext;
        private readonly ChildItemsAggregator _aggregator = new ChildItemsAggregator();
        private PositionController _bossPositionController;

        public Game Game
        {
            get => _game;
            private set
            {
                if (_game == value)
                    return;

                _game = value;

                if (_game != null)
                {
                    // Костыль, чтобы _canvas генерировал mouseMove
                    var back = new Rectangle { Width = 10000, Height = 10000, Fill = new SolidColorBrush(Color.FromArgb(1, 128, 128, 128)) };
                    _canvas.Children.Add(back);
                    Canvas.SetLeft(back, -back.Width / 2);
                    Canvas.SetTop(back, -back.Height / 2);

                    _arenaControl = new ArenaControl { Arena = _game.Arena };
                    _canvas.Children.Add(_arenaControl);

                    var w = _root.ActualWidth / _arenaControl.Width;
                    var h = _root.ActualHeight / _arenaControl.Height;
                    _scaleTransform.ScaleX = 0.95 * Math.Min(w, h);
                    _scaleTransform.ScaleY = _scaleTransform.ScaleX;
                    new ZoomController(_root, _canvas, _scaleTransform, _translateTransform);

                    new DragAndDropController(_root, _translateTransform).ToCenter();

                    _heroControl = new HeroControl { Hero = _game.Hero };
                    _canvas.Children.Add(_heroControl);
                    new PositionController(_heroControl, _game.Hero);
                    new DirectionController(_heroControl, _game.Hero.LookDirection);
                    new HeroDirectionController(_game.Hero, _canvas);
                }
            }
        }

        public AppContext AppContext
        {
            get => _appContext;
            set
            {
                if (_appContext == value)
                    return;

                _appContext = value;

                if (_appContext != null)
                {
                    Game = _appContext.Game;

                    _aggregator.Add(_appContext.Game.Hero);
                    _appContext.Game.ArenaChanged += Game_ArenaChanged;
                    Game_ArenaChanged(_appContext.Game, null, _appContext.Game.Arena);
                    new ChildItemsController(_aggregator, _appContext.ChildUiElementFactory, _canvas);
                }
            }
        }

        private void Game_ArenaChanged(Game game, Arena oldArena, Arena newArena)
        {
            if (oldArena != null)
            {
                _aggregator.Remove(oldArena.Boss);
                _canvas.Children.Remove(_bossControl);
                _bossPositionController.Dispose();
            }

            if (newArena != null)
            {
                _aggregator.Add(newArena.Boss);

                _bossControl = new BossControl { Boss = newArena.Boss };
                _canvas.Children.Add(_bossControl);
                _bossPositionController = new PositionController(_bossControl, newArena.Boss);
            }

            if (_arenaControl != null)
            {
                _arenaControl.Arena = newArena;
                Canvas.SetLeft(_arenaControl, -_arenaControl.Width / 2);
                Canvas.SetTop(_arenaControl, -_arenaControl.Height / 2);
            }
        }

        public GameControl()
        {
            InitializeComponent();
        }
    }
}
