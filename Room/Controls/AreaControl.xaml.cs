using Room.Core.Skills;

namespace Room.Controls
{
    public partial class AreaControl
    {
        public AreaControl()
        {
            InitializeComponent();
        }

        public AreaControl(RoundArea area): this()
        {
            Width = area.Bounds.Width;
            Height = area.Bounds.Height;
        }
    }
}
