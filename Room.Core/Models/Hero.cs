using Kalavarda.Primitives;
using Kalavarda.Primitives.Abstract;
using Kalavarda.Primitives.Geometry;

namespace Room.Core.Models
{
    public class Hero: IHasPosition
    {
        public PointF Position { get; } = new PointF();

        public AngleF LookDirection { get; } = new AngleF();

        public AngleF MoveDirection { get; } = new AngleF();

        public RangeF MoveSpeed { get; } = new RangeF { Max = 2 * 5000f / 3600 };

        public SizeF Size { get; } = new SizeF { Width = 0.6f, Height = 0.6f };
    }
}
