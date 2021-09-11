using System.Windows;
using Room.Core.Models;

namespace Room.Controls
{
    public partial class HeroControl
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
                    Width = _hero.Size.Width;
                    Height = _hero.Size.Height;

                    ResetShape();
                }
            }
        }

        private void ResetShape()
        {
            var w = _hero.Size.Width / 4;
            var h = _hero.Size.Height / 4;
            _polygon.Points.Clear();
            _polygon.Points.Add(new Point(w, 0));
            _polygon.Points.Add(new Point(2 * w, 0));
            _polygon.Points.Add(new Point(3 * w, 2 * h));
            _polygon.Points.Add(new Point(2 * w, 4 * h));
            _polygon.Points.Add(new Point(w, 4 * h));
        }

        public HeroControl()
        {
            InitializeComponent();
        }
    }
}
