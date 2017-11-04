using NUnit.Framework;
using rharel.Functional;
using rharel.M3PD.Common.Delegates;
using System;
using System.Collections.Generic;

namespace rharel.M3PD.Expectations.Timing.Tests
{
    [TestFixture]
    public sealed class ExtensionsTest
    {
        private static readonly string ID = "mock id";

        [Test]
        public void Test_EvaluateFor_WithInvalidArgs()
        {
            Assert.Throws<ArgumentNullException>(
                () => new List<SocialRule<int>>().EvaluateFor(null)
            );
        }
        [Test]
        public void Test_EvaluateFor_WithAnEmptySequence()
        {
            var rules = new List<SocialRule<int>>();

            Assert.IsFalse(rules.EvaluateFor(ID).IsSome());
        }
        [Test]
        public void Test_EvaluateFor_WithIrrelevantRules()
        {
            var rule = new SocialRule<int>(
                precondition: Predicates.Never,
                affects: Indicators<string>.All,
                implication: 42,
                weight: 1.0f
            );
            var rules = new List<SocialRule<int>> { rule };

            Assert.IsFalse(rules.EvaluateFor(ID).IsSome());
        }
        [Test]
        public void Test_EvaluateFor_WithUnaffectedAgents()
        {
            var rule = new SocialRule<int>(
                precondition: Predicates.Always,
                affects: Indicators<string>.None,
                implication: 42,
                weight: 1.0f
            );
            var rules = new List<SocialRule<int>> { rule };

            Assert.IsFalse(rules.EvaluateFor(ID).IsSome());
        }
        [Test]
        public void Test_EvaluateFor_WithCompetingRules()
        {
            var first_rule = new SocialRule<int>(
                precondition: Predicates.Always,
                affects: Indicators<string>.All,
                implication: 1,
                weight: 1.0f
            );
            var second_rule = new SocialRule<int>(
                precondition: Predicates.Always,
                affects: Indicators<string>.All,
                implication: 2,
                weight: 2.0f
            );

            var rules_ascending = new List<SocialRule<int>>
            {
                first_rule,
                second_rule
            };
            var rules_descending = new List<SocialRule<int>>
            {
                second_rule,
                first_rule
            };

            Assert.AreEqual(
                second_rule.Implication, 
                rules_ascending.EvaluateFor(ID).Unwrap()
            );
            Assert.AreEqual(
                second_rule.Implication,
                rules_descending.EvaluateFor(ID).Unwrap()
            );
        }
    }
}
