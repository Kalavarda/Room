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
                    Width = _arena.Size.Width;
                    Height = _arena.Size.Height;
                }
            }
        }

        public ArenaControl()
        {
            InitializeComponent();
        }
    }
}
