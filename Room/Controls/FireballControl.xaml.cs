using Room.Core.Skills;

namespace Room.Controls
{
    public partial class FireballControl
    {
        public FireballControl()
        {
            InitializeComponent();
        }

        public FireballControl(Fireball fireball): this()
        {
            Width = fireball.Bounds.Width;
            Height = fireball.Bounds.Height;
        }
    }
}
