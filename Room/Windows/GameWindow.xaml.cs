using System;
using System.Windows.Controls;
using System.Windows.Input;
using Kalavarda.Primitives.Controllers;
using Kalavarda.Primitives.Geometry;
using Kalavarda.Primitives.WPF.Controllers;
using Room.Controllers;
using Room.Controls;
using Room.Core.Models;
using Room.Processes;

namespace Room.Windows
{
    public partial class GameWindow
    {
        private ArenaControl _arenaControl;
        private HeroControl _heroControl;

        public Game Game { get; }

        public PointF MouseWorldPosition { get; } = new PointF();

        public GameWindow()
        {
            InitializeComponent();
        }

        public GameWindow(Game game): this()
        {
            Game = game ?? throw new ArgumentNullException(nameof(game));

            _arenaControl = new ArenaControl { Arena = game.Arena };
            _canvas.Children.Add(_arenaControl);
            Canvas.SetLeft(_arenaControl, -_arenaControl.Width / 2);
            Canvas.SetTop(_arenaControl, -_arenaControl.Height / 2);

            _heroControl = new HeroControl { Hero = game.Hero };
            _canvas.Children.Add(_heroControl);
            new PositionController(_heroControl, game.Hero);

            new ZoomController(_root, _canvas, _scaleTransform, _translateTransform);
            new DragAndDropController(_root, _translateTransform, _scaleTransform);

            new HeroDirectionController(game.Hero, _canvas);
            new DirectionController(_heroControl, game.Hero.LookDirection);
            new HeroMoveController(game.Hero, this, HeroMoveController.Mode.ByLook);
        }

        private void _canvas_OnMouseMove(object sender, MouseEventArgs e)
        {
            var screenPoint = PointToScreen(Mouse.GetPosition(this));
            var worldPoint = _canvas.PointFromScreen(screenPoint);
            MouseWorldPosition.Set((float)worldPoint.X, (float)worldPoint.Y);
        }
    }
}
