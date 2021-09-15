using System;
using System.Collections.Generic;

namespace Room.Core.Abstract
{
    public interface IGameItemsContainer
    {
        event Action<IGameItemType, long> Changed;

        long GetCount(IGameItemType itemType);

        IReadOnlyCollection<IGameItemType> AllTypes { get; }
    }

    public interface IGameItemsContainerExt: IGameItemsContainer
    {
        bool TryChangeCount(IGameItemType itemType, long count);
    }
}