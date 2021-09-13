using System.Collections.Generic;
using System.Linq;
using Kalavarda.Primitives.Abstract;
using Kalavarda.Primitives.Utils;
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
        private readonly Mock<IRandom> _random = new Mock<IRandom>();

        [TestCase(1, 100)]
        [TestCase(3, 200)]
        [TestCase(5, 400)]
        public void XP_Test(int level, long expectedXP)
        {
            _boss
                .Setup(b => b.Level)
                .Returns((ushort)level);

            var source = new AwardSource(new LevelMultiplier(), _random.Object);
            var awards = source.GetAwards(_boss.Object);
            Assert.AreEqual(expectedXP, awards.First(a => a.Key == GameItemTypes.XP).Value);
        }
    }
}
