using System.Collections.Generic;
using Kalavarda.Primitives.Abstract;

namespace Room.Core.Abstract
{
    public interface IFinesSource
    {
        /// <summary>
        /// Оштрафовать
        /// </summary>
        IReadOnlyDictionary<IHasName, long> Fine();
    }
}
