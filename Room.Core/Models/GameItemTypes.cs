using System;
using System.Diagnostics;
using Kalavarda.Primitives.Abstract;
using Room.Core.Abstract;

namespace Room.Core.Models
{
    [DebuggerDisplay("{Name}")]
    public class GameItemTypes: IGameItemType, IHasImage
    {
        public static IGameItemType XP { get; } = new GameItemTypes("Опыт");

        public static IGameItemType SmallHealthPotion { get; } = new GameItemTypes("Малое зелье HP")
        {
            UseInterval = TimeSpan.FromSeconds(5),
            ImageUri = new Uri(
                System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Resources\ItemTypes", "SmallHealthPotion.png"),
                UriKind.Absolute)
        };

        public string Name { get; }
        
        public TimeSpan UseInterval { get; private set; }
        
        public Uri ImageUri { get; private set; }

        public ushort? RequiredLevel => null;

        public ItemQuality Quality { get; } = ItemQuality.Ordinary;

        private GameItemTypes(string name)
        {
            Name = name;
        }
    }
}
