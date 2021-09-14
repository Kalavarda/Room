using Kalavarda.Primitives.Geometry;

namespace Room.Core.Models
{
    public class Arena
    {
        public Arena(Boss boss)
        {
            Boss = boss;

            Bounds = new RectBounds(new PointF());
        }

        public Boss Boss { get; }

        public BoundsF Bounds { get; }
    }
}
