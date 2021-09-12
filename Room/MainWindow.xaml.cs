using Room.Controllers;
using Room.Windows;

namespace Room
{
    public partial class MainWindow
    {
        private readonly AppContext _appContext;

        public MainWindow()
        {
            InitializeComponent();

            _appContext = new AppContext();

            Loaded += MainWindow_Loaded;
            Unloaded += MainWindow_Unloaded;
        }

        private void MainWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            var gameWindow = new GameWindow(_appContext) { Owner = this };
            new GameController(_appContext.Game, _appContext.AwardsSource, _appContext.FinesSource, gameWindow, _appContext.ArenaFactory, _appContext.Processor);
            gameWindow.ShowDialog();
        }

        private void MainWindow_Unloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            _appContext.Dispose();
        }
    }
}
