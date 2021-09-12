using Kalavarda.Primitives.Geometry;

namespace Room.Core.Abstract
{
    public interface ILooking
    {
        /// <summary>
        /// Направление взгляда
        /// </summary>
        AngleF LookDirection { get; }
    }
}