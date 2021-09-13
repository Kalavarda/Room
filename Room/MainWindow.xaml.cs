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
        private GameController _gameController;

        public MainWindow()
        {
            InitializeComponent();

            _appContext = new AppContext();
            _arenaSelector.ArenaFactory = _appContext.ArenaFactory;

            Loaded += MainWindow_Loaded;
            Unloaded += (sender, e) => _appContext.Dispose();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var logControl = new LogControl { ItemsContainer = _appContext.Game.Hero.ItemsContainer };
            ShowToolWindow(logControl, 400, 200, "Лог");
        }

        private void OnStartClick(object sender, RoutedEventArgs e)
        {
            var arena = _arenaSelector.SelectedArena;

            _appContext.Game.Hero.Ressurect();
            _appContext.Game.Arena = arena;
            _appContext.Game.Hero.Position.Set(-_appContext.Game.Arena.Bounds.Width / 4, _appContext.Game.Arena.Bounds.Height / 4);

            var gameWindow = new GameWindow(_appContext) { Owner = this, Title = arena.ToString() };
            _gameController = new GameController(_appContext.Game, _appContext.AwardsSource, _appContext.FinesSource, gameWindow, _appContext.Processor, _appContext.LevelMultiplier);
            gameWindow.Closed += GameWindow_Closed;
            gameWindow.ShowDialog();
        }

        private void GameWindow_Closed(object sender, System.EventArgs e)
        {
            _gameController.Dispose();
            _gameController = null;
            _arenaSelector.RefreshArenas();
        }
        
        internal void ShowToolWindow(UserControl content, int width, int height, string title, bool topmost = true)
        {
            new Window
            {
                Content = content,
                Owner = this,
                ShowInTaskbar = false,
                Width = width,
                Height = height,
                WindowStyle = WindowStyle.ToolWindow,
                Title = title,
                Background = Brushes.Black,
                Topmost = topmost
            }.Show();
        }
    }
}
