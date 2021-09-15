using System;
using System.Collections.Generic;
using Kalavarda.Primitives.Abstract;

namespace Room.Core.Abstract
{
    public interface IGameItemsContainer
    {
        event Action<IHasName, long> Changed;

        long GetCount(IHasName itemType);

        IReadOnlyCollection<IHasName> AllTypes { get; }
    }

    public interface IGameItemsContainerExt: IGameItemsContainer
    {
        bool TryChangeCount(IHasName itemType, long count);
    }
}