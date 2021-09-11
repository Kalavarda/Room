﻿using Room.Processes;
using Room.Windows;

namespace Room
{
    public partial class MainWindow
    {
        private readonly AppContext _appContext = new AppContext();

        public MainWindow()
        {
            InitializeComponent();

            Loaded += MainWindow_Loaded;
            Unloaded += MainWindow_Unloaded;
        }

        private void MainWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            _appContext.Processor.Add(new HeroMoveProcess(_appContext.Game.Hero));

            var gameWindow = new GameWindow(_appContext.Game) { Owner = this };
            gameWindow.ShowDialog();
        }

        private void MainWindow_Unloaded(object sender, System.Windows.RoutedEventArgs e)
        {
            _appContext.Dispose();
        }
    }
}