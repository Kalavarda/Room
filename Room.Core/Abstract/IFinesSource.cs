using System.Collections.Generic;

namespace Room.Core.Abstract
{
    public interface IFinesSource
    {
        /// <summary>
        /// Оштрафовать
        /// </summary>
        IReadOnlyDictionary<IGameItemType, long> Fine();
    }
}
