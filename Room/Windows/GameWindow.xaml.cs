using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Kalavarda.Primitives.WPF;
using Room.Controllers;
using Room.Core.Models;
using Room.Processes;

namespace Room.Windows
{
    public interface IGameWindow
    {
        void ShowInformation(string text, Action onClose);

        void ShowWarning(string text, Action onClose);

        void ShowGetReady(TimeSpan waitTime);
        
        void Close();
    }

    public partial class GameWindow : IGameWindow
    {
        private readonly AppContext _appContext;
        private HeroMoveProcess _heroMoveProcess;
        private HeroMoveController _heroMoveController;
        private SkillController _skillController;

        public GameWindow()
        {
            InitializeComponent();
        }

        public GameWindow(AppContext appContext): this()
        {
            _appContext = appContext ?? throw new ArgumentNullException(nameof(appContext));

            Loaded += GameWindow_Loaded;
        }

        private void GameWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _gameControl.AppContext = _appContext;

            var hero = _appContext.Game.Hero;

            _heroMoveProcess = new HeroMoveProcess(hero, _appContext.Game.Arena);
            _appContext.Processor.Add(_heroMoveProcess);
            _bossHP.Range = _appContext.Game.Arena.Boss.HP;

            _heroMoveController = new HeroMoveController(hero, this, HeroMoveController.Mode.ByLook);
            _skillController = new SkillController(hero, this, _appContext.Processor, _appContext.HeroSkillBinds);

            _heroHP.Range = hero.HP;
            _XP.Range = hero.XP;


            _skillControl1.Skill = _appContext.HeroSkillBinds.GetSkill(Hero.SkillKey_Fireball);
            _skillControl1.Bind = _appContext.HeroSkillBinds.GetBind(Hero.SkillKey_Fireball);

            _skillControl2.Skill = _appContext.HeroSkillBinds.GetSkill(Hero.SkillKey_Healing);
            _skillControl2.Bind = _appContext.HeroSkillBinds.GetBind(Hero.SkillKey_Healing);

            _skillControl3.Skill = _appContext.HeroSkillBinds.GetSkill(Hero.SkillKey_Teleport_Backward);
            _skillControl3.Bind = _appContext.HeroSkillBinds.GetBind(Hero.SkillKey_Teleport_Backward);

            _skillControl4.Skill = _appContext.HeroSkillBinds.GetSkill(Hero.SkillKey_Teleport_Forward);
            _skillControl4.Bind = _appContext.HeroSkillBinds.GetBind(Hero.SkillKey_Teleport_Forward);

            _skillControl5.Skill = _appContext.HeroSkillBinds.GetSkill(Hero.SkillKey_Use_ + GameItemTypes.SmallHealthPotion.Name);
            _skillControl5.Bind = _appContext.HeroSkillBinds.GetBind(Hero.SkillKey_Use_ + GameItemTypes.SmallHealthPotion.Name);

            TuneControls();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            _heroMoveProcess.Stop();
            _heroMoveController.Dispose();
            _skillController.Dispose();

            base.OnClosing(e);
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

        public void ShowInformation(string text, Action onClose)
        {
            this.Do(() =>
            {
                MessageBox.Show(text, string.Empty, MessageBoxButton.OK, MessageBoxImage.Information);
                onClose?.Invoke();
            });
        }

        public void ShowWarning(string text, Action onClose)
        {
            this.Do(() =>
            {
                MessageBox.Show(text, string.Empty, MessageBoxButton.OK, MessageBoxImage.Warning);
                onClose?.Invoke();
            });
        }

        public void ShowGetReady(TimeSpan waitTime)
        {
            _bGetReady.Visibility = waitTime > TimeSpan.FromSeconds(0.5) ? Visibility.Visible : Visibility.Collapsed;
            _tbGetReady.Text = Math.Round(waitTime.TotalSeconds) + " sec.";
        }
    }
}
