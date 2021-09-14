using Kalavarda.Primitives.Utils;
using Kalavarda.Primitives.WPF;
using Room.Core.Models;

namespace Room.Controls
{
    public partial class HeroInfo
    {
        private Hero _hero;

        public Hero Hero
        {
            get => _hero;
            set
            {
                if (_hero == value)
                    return;

                _hero = value;

                if (_hero != null)
                {
                    _hero.LevelChanged += Hero_LevelChanged;
                    Hero_LevelChanged();

                    _hero.XP.ValueChanged += XP_ValueChanged;
                    XP_ValueChanged(_hero.XP);
                }
            }
        }

        private void XP_ValueChanged(Kalavarda.Primitives.RangeF xp)
        {
            this.Do(() =>
            {
                _tbXP.Text = $"{_hero.XP.Value.ToStr()} / {_hero.XP.Max.ToStr()}";
            });
        }

        private void Hero_LevelChanged()
        {
            this.Do(() =>
            {
                _tbLevel.Text = _hero.Level.ToString();
            });
        }

        public HeroInfo()
        {
            InitializeComponent();
        }
    }
}
