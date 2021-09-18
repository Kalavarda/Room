using Kalavarda.Primitives;
using Kalavarda.Primitives.Abstract;
using Kalavarda.Primitives.Skills;
using Moq;
using NUnit.Framework;
using Room.Core.Abstract;
using Room.Core.Impl;
using Room.Core.Models;

namespace Room.Core.Tests
{
    public class HpChanger_Tests
    {
        private readonly Mock<ILevelMultiplier> _levelMultiplier = new Mock<ILevelMultiplier>();
        private readonly Mock<ISkilled> _initializer = new Mock<ISkilled>();

        public HpChanger_Tests()
        {
            _levelMultiplier
                .Setup(lm => lm.GetRatio(It.IsAny<float>()))
                .Returns<float>(p => 1 + p);
        }

        [TestCase(false, -10)]
        [TestCase(true, 0)]
        public void InvFrame_Test(bool invFrame, float expectedHp)
        {
            var hp = new RangeF { Max = 100, Value = 50 };
            var targetCreature = new Mock<ICreatureExt>();
            targetCreature
                .Setup(tc => tc.HP)
                .Returns(hp);
            targetCreature.As<IHasModifiers>()
                .Setup(hm => hm.Modifiers)
                .Returns(new Modifiers { InvFrame = invFrame });

            var initializer = new Mock<ISkilled>();

            var hpChanger = new HpChanger(_levelMultiplier.Object);
            var result = hpChanger.CalculateHpChange(targetCreature.Object, -10, initializer.Object);
            Assert.AreEqual(expectedHp, result, 0.01f);
        }

        [TestCase(0, -10)]
        [TestCase(20, -30)]
        public void Attack_Test(float attack, float expectedHp)
        {
            var hp = new RangeF { Max = 100, Value = 50 };
            var targetCreature = new Mock<ICreatureExt>();
            targetCreature
                .Setup(tc => tc.HP)
                .Returns(hp);

            _initializer.As<IHasModifiers>()
                .Setup(hm => hm.Modifiers)
                .Returns(new Modifiers { Attack = attack });

            var hpChanger = new HpChanger(_levelMultiplier.Object);
            var result = hpChanger.CalculateHpChange(targetCreature.Object, -10, _initializer.Object);
            Assert.AreEqual(expectedHp, result, 0.01f);
        }

        [TestCase(0, -10)]
        [TestCase(10, -5)]
        public void Defence_Test(float defence, float expectedHp)
        {
            var hp = new RangeF { Max = 100, Value = 50 };
            var targetCreature = new Mock<ICreatureExt>();
            targetCreature
                .Setup(tc => tc.HP)
                .Returns(hp);
            targetCreature.As<IHasModifiers>()
                .Setup(hm => hm.Modifiers)
                .Returns(new Modifiers { Defence = defence });

            var hpChanger = new HpChanger(_levelMultiplier.Object);
            var result = hpChanger.CalculateHpChange(targetCreature.Object, -10, new Mock<ISkilled>().Object);
            Assert.AreEqual(expectedHp, result, 0.01f);
        }
    }
}
