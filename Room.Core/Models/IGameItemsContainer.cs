using System;
using System.Collections.Generic;
using Room.Core.Abstract;

namespace Room.Core.Models
{
    public interface IGameItemsContainer
    {
        event Action<IGameItemType, float> Changed;
    }

    public interface IGameItemsContainerExt: IGameItemsContainer
    {
        void Add(IGameItemType itemType, float count);

        void Add(IReadOnlyDictionary<IGameItemType, float> awards);
    }
}