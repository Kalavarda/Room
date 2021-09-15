using System;
using Kalavarda.Primitives.Abstract;

namespace Room.Core.Abstract
{
    public interface IGameItemType: IHasName
    {
        string Name { get; }

        TimeSpan UseInterval { get; }

        ushort? RequiredLevel { get; }

        ItemQuality Quality { get; }
    }

    public enum ItemQuality
    {
        /// <summary>
        /// Хлам
        /// </summary>
        Junk,

        /// <summary>
        /// Обычное
        /// </summary>
        Ordinary,

        /// <summary>
        /// Хорошее
        /// </summary>
        Good,

        /// <summary>
        /// Редкое
        /// </summary>
        Rare,

        Legendary,

        Epic
    }
}
