using NUnit.Framework;
using rharel.M3PD.Common.Delegates;
using System;

namespace rharel.M3PD.Expectations.Timing.Tests
{
    [TestFixture]
    public sealed class SocialRuleTest
    {
        private static readonly Predicate PRECONDITION = Predicates.Always;
        private static readonly Indicator<string> AFFECTED_INDICATOR = (
            Indicators<string>.All
        );
        private static readonly int IMPLICATION = 1;
        private static readonly float WEIGHT = 1.0f;

        [Test]
        public void Test_Constructor_WithInvalidArgs()
        {
            Assert.Throws<ArgumentNullException>(
                () => new SocialRule<int>(
                    null,
                    AFFECTED_INDICATOR,
                    IMPLICATION,
                    WEIGHT
                )
            );
            Assert.Throws<ArgumentNullException>(
                () => new SocialRule<int>(
                    PRECONDITION,
                    null,
                    IMPLICATION,
                    WEIGHT
                )
            );
            Assert.Throws<ArgumentException>(
                () => new SocialRule<int>(
                    PRECONDITION,
                    AFFECTED_INDICATOR,
                    IMPLICATION,
                    -1.0f
                )
            );
        }
        [Test]
        public void Test_Constructor()
        {
            var rule = new SocialRule<int>(
                PRECONDITION,
                AFFECTED_INDICATOR,
                IMPLICATION,
                WEIGHT
            );

            Assert.AreEqual(IMPLICATION, rule.Implication);
            Assert.AreEqual(WEIGHT, rule.Weight);
        }

        [Test]
        public void Test_IsRelevant()
        {
            bool delegate_invoked = false;

            var rule = new SocialRule<int>(
                () => { return delegate_invoked = true; },
                AFFECTED_INDICATOR,
                IMPLICATION
            );

            Assert.IsTrue(rule.IsRelevant());
            Assert.IsTrue(delegate_invoked);
        }

        [Test]
        public void Test_IsAffecting()
        {
            var id = "mock id";
            bool delegate_invoked = false;

            var rule = new SocialRule<int>(
                PRECONDITION,
                value => 
                {
                    Assert.AreEqual(value, id);
                    return delegate_invoked = true;
                },
                IMPLICATION
            );

            Assert.IsTrue(rule.IsAffecting(id));
            Assert.IsTrue(delegate_invoked);
        }

        [Test]
        public void Test_Equality()
        {
            var original = new SocialRule<int>(
                PRECONDITION, 
                AFFECTED_INDICATOR,
                IMPLICATION, 
                WEIGHT
            );
            var good_copy = new SocialRule<int>(
                PRECONDITION,
                AFFECTED_INDICATOR,
                original.Implication,
                original.Weight
            );
            var flawed_precondition_copy = new SocialRule<int>(
                Predicates.Never,
                AFFECTED_INDICATOR,
                original.Implication,
                original.Weight
            );
            var flawed_affected_indicator_copy = new SocialRule<int>(
                PRECONDITION,
                Indicators<string>.None,
                original.Implication,
                original.Weight
            );
            var flawed_implication_copy = new SocialRule<int>(
                PRECONDITION,
                AFFECTED_INDICATOR,
                original.Implication + 1,
                original.Weight
            );
            var flawed_weight_copy = new SocialRule<int>(
                PRECONDITION,
                AFFECTED_INDICATOR,
                original.Implication,
                original.Weight + 1
            );

            Assert.AreNotEqual(original, null);
            Assert.AreNotEqual(original, "incompatible type");
            Assert.AreNotEqual(original, flawed_precondition_copy);
            Assert.AreNotEqual(original, flawed_affected_indicator_copy);
            Assert.AreNotEqual(original, flawed_implication_copy);
            Assert.AreNotEqual(original, flawed_weight_copy);

            Assert.AreEqual(original, original);
            Assert.AreEqual(original, good_copy);
        }
    }
}
