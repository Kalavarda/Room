using System;
using Kalavarda.Primitives;
using Kalavarda.Primitives.Process;
using Kalavarda.Primitives.Skills;
using Room.Core.Abstract;
using Room.Core.Models;

namespace Room.Core.Skills
{
    public class UseItemSkill: ISkill, IHasKey, IHasCount<long>
    {
        private readonly Hero _hero;
        private readonly TimeLimiter _timeLimiter;
        private readonly IGameItemType _itemType;

        public string Name => _itemType.Name;

        public float MaxDistance => 0;

        public ITimeLimiter TimeLimiter => _timeLimiter;

        public UseItemSkill(Hero hero, IGameItemType itemType)
        {
            _hero = hero ?? throw new ArgumentNullException(nameof(hero));
            _itemType = itemType ?? throw new ArgumentNullException(nameof(itemType));
            _timeLimiter = new TimeLimiter(itemType.UseInterval);

            _hero.ItemsContainer.Changed += ItemsContainer_Changed;
        }

        private void ItemsContainer_Changed(IGameItemType itemType, long count)
        {
            if (itemType != _itemType)
                return;

            CountChanged?.Invoke(this);
        }

        public IProcess Use(ISkilled initializer)
        {
            if (_itemType == GameItemTypeTypes.SmallHealthPotion)
                if (_hero.ItemsContainer.TryChangeCount(_itemType, -1))
                    _timeLimiter.Do(() =>
                    {
                        _hero.ChangeHP(5, initializer, this);
                    });
            return null;
        }

        public string Key => Hero.SkillKey_Use_ + _itemType.Name;

        public long Count => _hero.ItemsContainer.GetCount(_itemType);

        public long? Max => null;
        
        public event Action<IHasCount<long>> CountChanged;
    }
}
