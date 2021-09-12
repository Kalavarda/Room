using Room.Core.Models;

namespace Room.Core.Abstract
{
    public interface IHasModifiers
    {
        Modifiers Modifiers { get; }
    }
}