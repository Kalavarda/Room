using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Room.Core.Abstract;

namespace Room.Core.Models
{
    public class GameItemsContainer : IGameItemsContainerExt
    {
        private readonly IDictionary<IGameItemType, long> _dictionary = new ConcurrentDictionary<IGameItemType, long>();

        public bool TryChangeCount(IGameItemType itemType, long count)
        {
            if (itemType == null) throw new ArgumentNullException(nameof(itemType));

            return count < 0
                ? Subtract(itemType, -count)
                : Add(itemType, count);
        }

        private bool Add(IGameItemType itemType, long count)
        {
            // TODO: проверить лимит владения

            if (_dictionary.ContainsKey(itemType))
                _dictionary[itemType] += count;
            else
                _dictionary.Add(itemType, count);

            Changed?.Invoke(itemType, count);

            return true;
        }

        private bool Subtract(IGameItemType itemType, long count)
        {
            if (_dictionary.ContainsKey(itemType))
            {
                var newValue = _dictionary[itemType] - count;
                if (newValue < 0)
                    return false;

                _dictionary[itemType] = newValue;
                Changed?.Invoke(itemType, count);
                return true;
            }
            else
                return false;
        }

        public event Action<IGameItemType, long> Changed;
        
        public long GetCount(IGameItemType itemType)
        {
            if (_dictionary.ContainsKey(itemType))
                return _dictionary[itemType];
            else
                return 0;
        }
    }
}
