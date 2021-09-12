using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Room.Core.Abstract;

namespace Room.Core.Models
{
    public class GameItemsContainer : IGameItemsContainerExt
    {
        private readonly IDictionary<IGameItemType, float> _dictionary = new ConcurrentDictionary<IGameItemType, float>();

        public void Add(IGameItemType itemType, float count)
        {
            if (_dictionary.ContainsKey(itemType))
                _dictionary[itemType] += count;
            else
                _dictionary.Add(itemType, count);
            Changed?.Invoke(itemType, count);
        }

        public void Add(IReadOnlyDictionary<IGameItemType, float> awards)
        {
            if (awards == null) throw new ArgumentNullException(nameof(awards));
            foreach (var award in awards)
                Add(award.Key, award.Value);
        }

        public event Action<IGameItemType, float> Changed;
    }
}
