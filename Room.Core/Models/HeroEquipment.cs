using System;
using System.Collections.Generic;
using System.Linq;
using Room.Core.Abstract;

namespace Room.Core.Models
{
    public class HeroEquipment: IHasModifiers
    {
        private Modifiers _modifiers;

        /// <summary>
        /// Ожерелье
        /// </summary>
        public IEquipmentItem Necklace { get; private set; }

        /// <summary>
        /// Пояс
        /// </summary>
        public IEquipmentItem Belt { get; private set; }

        public HeroEquipment()
        {
            RecalcModifiers();
        }

        public bool IsEquiped(IEquipmentItem equipItem)
        {
            if (equipItem == null) throw new ArgumentNullException(nameof(equipItem));
            return Necklace == equipItem || Belt == equipItem;
        }

        public bool TryEquip(IEquipmentItem equipItem)
        {
            if (equipItem == null) throw new ArgumentNullException(nameof(equipItem));
            
            if (equipItem.Type == EquipmentType.Necklace)
                if (Necklace == null)
                {
                    Necklace = equipItem;
                    RecalcModifiers();
                    return true;
                }
            
            if (equipItem.Type == EquipmentType.Belt)
                if (Belt == null)
                {
                    Belt = equipItem;
                    RecalcModifiers();
                    return true;
                }

            return false;
        }

        public IReadOnlyCollection<IEquipmentItem> AllItems
        {
            get
            {
                return new [] { Necklace, Belt }.Where(i => i != null).ToArray();
            }
        }

        public IModifiers Modifiers => _modifiers;

        private void RecalcModifiers()
        {
            _modifiers = Models.Modifiers.Combine(AllItems.OfType<IHasModifiers>().Select(hm => hm.Modifiers).ToArray());
        }
    }
}
