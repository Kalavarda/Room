using System;
using Room.Core.Skills;

namespace Room.Controls
{
    public partial class BombControl
    {
        private readonly Bomb _bomb;

        public BombControl()
        {
            InitializeComponent();
        }

        public BombControl(Bomb bomb): this()
        {
            _bomb = bomb ?? throw new ArgumentNullException(nameof(bomb));

            Width = bomb.Bounds.Width;
            Height = bomb.Bounds.Height;
        }
    }
}
