namespace Room.Core.Abstract
{
    public interface IModifiers
    {
        /// <summary>
        /// Invulnerability frame (неуязвимость)
        /// </summary>
        bool InvFrame { get; set; }

        /// <summary>
        /// Множитель атаки
        /// </summary>
        float Attack { get; set; }

        /// <summary>
        /// Множитель защиты
        /// </summary>
        float Defence { get; set; }
    }
}