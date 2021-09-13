using System;

namespace Room.Core.Abstract
{
    public interface IGameItemsContainer
    {
        event Action<IGameItemType, long> Changed;

        long GetCount(IGameItemType itemType);
    }

    public interface IGameItemsContainerExt: IGameItemsContainer
    {
        bool TryChangeCount(IGameItemType itemType, long count);
    }
}