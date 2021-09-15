using NUnit.Framework;
using Room.Core.Models;

namespace Room.Core.Tests
{
    public class GameItemsContainer_Tests
    {
        [TestCase(100, true)]
        [TestCase(-100, false)]
        public void TryChangeXP_Test(long xp, bool success)
        {
            var container = new GameItemsContainer();
            var result = container.TryChangeCount(XP.Instance, xp);
            Assert.AreEqual(success, result);
            result = container.TryChangeCount(XP.Instance, xp);
            Assert.AreEqual(success, result);
        }
    }
}
