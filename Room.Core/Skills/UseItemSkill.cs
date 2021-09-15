using System;
using Kalavarda.Primitives;
using Kalavarda.Primitives.Abstract;
using Kalavarda.Primitives.Process;
using Kalavarda.Primitives.Skills;
using Room.Core.Abstract;
using Room.Core.Models;

namespace Room.Core.Skills
{
    public class UseItemSkill: ISkill, IHasKey, IHasCount<long>, IHasImage
    {
        private readonly Hero _hero;
        private readonly TimeLimiter _timeLimiter;
        private readonly IGameItemType _itemType;
        private readonly IHpChanger _hpChanger;

        public string Name => _itemType.Name;

        public float MaxDistance => 0;

        public ITimeLimiter TimeLimiter => _timeLimiter;

        public UseItemSkill(Hero hero, IGameItemType itemType, IHpChanger hpChanger)
        {
            _hero = hero ?? throw new ArgumentNullException(nameof(hero));
            _itemType = itemType ?? throw new ArgumentNullException(nameof(itemType));
            _hpChanger = hpChanger ?? throw new ArgumentNullException(nameof(hpChanger));
            _timeLimiter = new TimeLimiter(itemType.UseInterval);

            _hero.Bag.Changed += ItemsContainer_Changed;
        }

        private void ItemsContainer_Changed(IGameItemType itemType, long count)
        {
            if (itemType != _itemType)
                return;

            CountChanged?.Invoke(this);
        }

        public IProcess Use(ISkilled initializer)
        {
            if (_itemType == GameItemTypes.SmallHealthPotion)
                if (_hero.Bag.TryChangeCount(_itemType, -1))
                    _timeLimiter.Do(() =>
                    {
                        _hpChanger.ApplyChange(_hero, 5, initializer, this);
                    });
            return null;
        }

        public string Key => Hero.SkillKey_Use_ + _itemType.Name;

        public long Count => _hero.Bag.GetCount(_itemType);

        public long? Max => null;
        
        public event Action<IHasCount<long>> CountChanged;
        
        public Uri ImageUri => _itemType.ImageUri;
    }
}
