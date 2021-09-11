using System;
using System.Windows;
using System.Windows.Input;
using Room.Controllers;

namespace Room.Windows
{
    public partial class GameWindow
    {
        private readonly AppContext _appContext;

        public GameWindow()
        {
            InitializeComponent();
        }

        public GameWindow(AppContext appContext): this()
        {
            _appContext = appContext ?? throw new ArgumentNullException(nameof(appContext));

            _heroHP.Range = appContext.Game.Hero.HP;
            _bossHP.Range = appContext.Game.Boss.HP;

            Loaded += GameWindow_Loaded;
        }

        private void GameWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _gameControl.AppContext = _appContext;
            new HeroMoveController(_appContext.Game.Hero, this, HeroMoveController.Mode.Simple);
            new SkillController(_appContext.Game.Hero, this, _appContext.Processor);

            TuneControls();
        }

        private void TuneControls()
        {
            _bPause.Visibility = _appContext.Processor.Paused ? Visibility.Visible : Visibility.Collapsed;
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.Handled)
                return;

            if (e.Key == Key.Pause)
            {
                _appContext.Processor.Paused = !_appContext.Processor.Paused;
                TuneControls();
            }
        }
    }
}
