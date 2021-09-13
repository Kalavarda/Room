using System;

namespace Room.Core.Abstract
{
    public interface IGameItemType
    {
        string Name { get; }

        TimeSpan UseInterval { get; }
    }
}
