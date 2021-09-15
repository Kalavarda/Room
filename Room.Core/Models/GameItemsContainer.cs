using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Kalavarda.Primitives.Abstract;
using Room.Core.Abstract;

namespace Room.Core.Models
{
    public class GameItemsContainer : IGameItemsContainerExt
    {
        private readonly IDictionary<IHasName, long> _dictionary = new ConcurrentDictionary<IHasName, long>();

        public bool TryChangeCount(IHasName itemType, long count)
        {
            if (itemType == null) throw new ArgumentNullException(nameof(itemType));

            if (count == 0)
                return true;

            return count < 0
                ? Subtract(itemType, -count)
                : Add(itemType, count);
        }

        private bool Add(IHasName itemType, long count)
        {
            // TODO: проверить лимит владения

            if (_dictionary.ContainsKey(itemType))
                _dictionary[itemType] += count;
            else
                _dictionary.Add(itemType, count);

            Changed?.Invoke(itemType, count);

            return true;
        }

        private bool Subtract(IHasName itemType, long count)
        {
            if (_dictionary.ContainsKey(itemType))
            {
                var newValue = _dictionary[itemType] - count;
                if (newValue < 0)
                    return false;

                _dictionary[itemType] = newValue;
                Changed?.Invoke(itemType, -count);
                return true;
            }
            else
                return false;
        }

        public event Action<IHasName, long> Changed;
        
        public long GetCount(IHasName itemType)
        {
            if (_dictionary.ContainsKey(itemType))
                return _dictionary[itemType];
            else
                return 0;
        }

        public IReadOnlyCollection<IHasName> AllTypes => _dictionary.Where(p => p.Value > 0).Select(p => p.Key).ToArray();
    }
}
