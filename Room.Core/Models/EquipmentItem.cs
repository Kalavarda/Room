using System;
using System.Diagnostics;
using Kalavarda.Primitives.Abstract;
using Room.Core.Abstract;

namespace Room.Core.Models
{
    [DebuggerDisplay("{Name}")]
    public class EquipmentItem: IEquipmentItem, IHasImage, IHasModifiers
    {
        public EquipmentItem(EquipmentType type, string name, ushort requiredLevel)
        {
            Type = type;
            Name = name;
            RequiredLevel = requiredLevel;
        }

        public string Name { get; }

        public TimeSpan UseInterval => TimeSpan.Zero;
        
        public Uri ImageUri { get; private set; }
        
        public ushort? RequiredLevel { get; }

        public EquipmentType Type { get; }

        public ItemQuality Quality { get; private set; }

        public static IEquipmentItem OldNecklace { get; } = new EquipmentItem(EquipmentType.Necklace, "Старое ожерелье", 1)
        {
            Quality = ItemQuality.Junk,
            ImageUri = new Uri(
                System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Resources\ItemTypes", "OldNecklace.jpg"),
                UriKind.Absolute),
            Modifiers =
            {
                Defence = 1
            }
        };

        public static IEquipmentItem OldBelt { get; } = new EquipmentItem(EquipmentType.Belt, "Старый пояс", 1)
        {
            Quality = ItemQuality.Junk,
            ImageUri = new Uri(
                System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Resources\ItemTypes", "OldBelt.jpg"),
                UriKind.Absolute),
            Modifiers =
            {
                Attack = 1
            }
        };

        public IModifiers Modifiers { get; } = new Modifiers();
    }
}
