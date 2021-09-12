using System;
using Kalavarda.Primitives.WPF;
using Room.Core.Skills;

namespace Room.Controls
{
    public partial class AreaControl
    {
        private readonly RoundArea _area;

        public AreaControl()
        {
            InitializeComponent();
        }

        public AreaControl(RoundArea area): this()
        {
            _area = area ?? throw new ArgumentNullException(nameof(area));
            Width = area.Bounds.Width;
            Height = area.Bounds.Height;
            _scale.CenterX = Width / 2;
            _scale.CenterY = Height / 2;

            _area.RemainChanged += _area_RemainChanged;
            Unloaded += (sender, e) => _area.RemainChanged -= _area_RemainChanged;
            _area_RemainChanged(_area);
        }

        private void _area_RemainChanged(AreaBase area)
        {
            this.Do(() =>
            {
                var r = area.Remain.TotalSeconds / area.FullRemain.TotalSeconds;
                _scale.ScaleX = 0.5 + (1 - r) /2;
                _scale.ScaleY = _scale.ScaleX;
                _timerEllipse.Opacity = 1 - r;
            });
        }
    }
}
