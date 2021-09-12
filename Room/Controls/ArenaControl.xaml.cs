using Room.Core.Models;

namespace Room.Controls
{
    public partial class ArenaControl
    {
        private Arena _arena;

        public Arena Arena
        {
            get => _arena;
            set
            {
                if (_arena == value)
                    return;

                _arena = value;

                if (_arena != null)
                {
                    Width = _arena.Bounds.Width;
                    Height = _arena.Bounds.Height;
                }
            }
        }

        public ArenaControl()
        {
            InitializeComponent();
        }
    }
}
