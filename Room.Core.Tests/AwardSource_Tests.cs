using System.Linq;
using Moq;
using NUnit.Framework;
using Room.Core.Abstract;
using Room.Core.Impl;
using Room.Core.Models;

namespace Room.Core.Tests
{
    public class AwardSource_Tests
    {
        private readonly Mock<IHasLevel> _boss = new Mock<IHasLevel>();

        [TestCase(1, 100)]
        [TestCase(3, 200)]
        [TestCase(5, 400)]
        public void XP_Test(int level, float expectedXP)
        {
            _boss
                .Setup(b => b.Level)
                .Returns((ushort)level);

            var source = new AwardSource(new LevelMultiplier());
            var awards = source.GetAwards(_boss.Object);
            Assert.AreEqual(expectedXP, awards.First(a => a.Key == GameItemTypeTypes.XP).Value, 0.1f);
        }
    }
}
