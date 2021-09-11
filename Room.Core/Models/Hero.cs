using Kalavarda.Primitives;
using Kalavarda.Primitives.Abstract;
using Kalavarda.Primitives.Geometry;

namespace Room.Core.Models
{
    public class Hero: IHasPosition, IHasBounds
    {
        public PointF Position => Bounds.Position;

        public AngleF LookDirection { get; } = new AngleF();

        public AngleF MoveDirection { get; } = new AngleF();

        public RangeF MoveSpeed { get; } = new RangeF { Max = 2 * 5000f / 3600 };

        public SizeF Size => Bounds.Size;
        
        public BoundsF Bounds { get; } = new RoundBounds(new PointF(), 0.6f);
    }
}
