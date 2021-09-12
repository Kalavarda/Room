﻿using Room.Processes;
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
            _appContext.Processor.Add(new HeroMoveProcess(_appContext.Game.Hero, _appContext.Game.Arena));
            _appContext.Processor.Add(new BossProcess(_appContext.Game.Arena.Boss, _appContext.Game, _appContext.Processor));

            var gameWindow = new GameWindow(_appContext) { Owner = this };
            gameWindow.ShowDialog();
        }

        private void MainWindow_Unloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            _appContext.Dispose();
        }
    }
}
