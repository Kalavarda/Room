using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Room.Controllers;
using Room.Controls;
using Room.Windows;

namespace Room
{
    public partial class MainWindow
    {
        private readonly AppContext _appContext;
        private readonly GameController _gameController;

        public MainWindow()
        {
            InitializeComponent();

            _appContext = new AppContext();
            _arenaSelector.Hero = _appContext.Game.Hero;
            _arenaSelector.ArenaFactory = _appContext.ArenaFactory;

            Loaded += MainWindow_Loaded;
            Unloaded += (sender, e) =>
            {
                _appContext.Dispose();
                _gameController.Dispose();
            };

            _heroInfo.Hero = _appContext.Game.Hero;

            _gameController = new GameController(_appContext.Game, _appContext.AwardsSource, _appContext.FinesSource, _appContext.Processor, _appContext.LevelMultiplier);
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var logControl = new LogControl { Hero = _appContext.Game.Hero };
            ShowToolWindow(logControl, 400, 200, "Лог");
        }

        private void OnStartClick(object sender, RoutedEventArgs e)
        {
            var arena = _arenaSelector.SelectedArena;
            if (arena == null)
                return;

            _appContext.Game.Hero.Ressurect();
            _appContext.Game.Arena = arena;
            _appContext.Game.Hero.Position.Set(-_appContext.Game.Arena.Bounds.Width / 4, _appContext.Game.Arena.Bounds.Height / 4);

            var gameWindow = new GameWindow(_appContext) { Owner = this, Title = arena.ToString() };
            gameWindow.Closed += GameWindow_Closed;
            _gameController.GameWindow = gameWindow;
            gameWindow.ShowDialog();
        }

        private void GameWindow_Closed(object sender, System.EventArgs e)
        {
            _arenaSelector.RefreshArenas();
        }
        
        internal void ShowToolWindow(UserControl content, int width, int height, string title)
        {
            var window = new Window
            {
                Content = content,
                Owner = this,
                ShowInTaskbar = false,
                Width = width,
                Height = height,
                WindowStyle = WindowStyle.ToolWindow,
                Title = title,
                Background = Brushes.Black
            };
            window.Show();
        }
    }
}
