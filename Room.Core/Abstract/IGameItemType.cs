using System;

namespace Room.Core.Abstract
{
    public interface IGameItemType
    {
        string Name { get; }

        TimeSpan UseInterval { get; }

        Uri ImageUri { get; }

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
