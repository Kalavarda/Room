using Room.Core.Models;

namespace Room.Controls
{
    public partial class BossControl
    {
        private Boss _boss;

        public Boss Boss
        {
            get => _boss;
            set
            {
                if (_boss == value)
                    return;

                _boss = value;

                if (_boss != null)
                {
                    Width = _boss.Bounds.Size.Width;
                    Height = _boss.Bounds.Size.Height;
                }
            }
        }

        public BossControl()
        {
            InitializeComponent();
        }
    }
}
