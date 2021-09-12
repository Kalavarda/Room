using Kalavarda.Primitives.Geometry;

namespace Room.Core.Models
{
    public class Arena
    {
        public Arena(Boss boss)
        {
            Boss = boss;
        }

        public SizeF Size { get; } = new SizeF();

        public Boss Boss { get; }
    }
}
