using System.Windows.Controls;
using Kalavarda.Primitives.Controllers;
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
                    _arenaControl = new ArenaControl { Arena = _game.Arena };
                    _canvas.Children.Add(_arenaControl);
                    Canvas.SetLeft(_arenaControl, -_arenaControl.Width / 2);
                    Canvas.SetTop(_arenaControl, -_arenaControl.Height / 2);

                    new ZoomController(_root, _canvas, _scaleTransform, _translateTransform);
                    new DragAndDropController(_root, _translateTransform, _scaleTransform);

                    _bossControl = new BossControl { Boss = _game.Boss };
                    _canvas.Children.Add(_bossControl);
                    new PositionController(_bossControl, _game.Boss);

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
                    new ChildItemsController(_appContext.Game.Boss, _appContext.ChildUiElementFactory, _canvas);
                }
            }
        }

        public GameControl()
        {
            InitializeComponent();
        }
    }
}
