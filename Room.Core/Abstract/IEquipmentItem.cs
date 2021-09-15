namespace Room.Core.Abstract
{
    public interface IEquipmentItem: IGameItemType
    {
        public EquipmentType Type { get; }
    }

    public enum EquipmentType
    {
        /// <summary>
        /// Ожерелье
        /// </summary>
        Necklace,

        /// <summary>
        /// Пояс
        /// </summary>
        Belt
    }
}
