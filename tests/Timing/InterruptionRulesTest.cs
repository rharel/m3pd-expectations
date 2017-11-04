using NUnit.Framework;
using rharel.M3PD.Common.Collections;
using rharel.M3PD.Common.Delegates;
using System;
using System.Collections.Generic;

namespace rharel.M3PD.Expectations.Timing.Tests
{
    [TestFixture]
    public sealed class InterruptionRulesTest
    {
        private static readonly
            IEnumerable<SocialRule<InterruptionInitiation>>
            INITIATION =
            new SocialRule<InterruptionInitiation>[1]
            {
                new SocialRule<InterruptionInitiation>(   
                    Predicates.Never,
                    Indicators<string>.None,
                    InterruptionInitiation.Interrupt
                )
            };
        private static readonly
            IEnumerable<SocialRule<InterruptionResponse>>
            RESPONSE =
            new SocialRule<InterruptionResponse>[1]
            {
                new SocialRule<InterruptionResponse>(
                    Predicates.Never,
                    Indicators<string>.None,
                    InterruptionResponse.Surrender
                )
            };

        private InterruptionRules _rules;

        [SetUp]
        public void Setup()
        {
            _rules = new InterruptionRules(INITIATION, RESPONSE);
        }

        [Test]
        public void Test_Constructor_WithInvalidArgs()
        {
            Assert.Throws<ArgumentNullException>(
                () => new InterruptionRules(null, RESPONSE)
            );
            Assert.Throws<ArgumentNullException>(
                () => new InterruptionRules(INITIATION, null)
            );
        }
        [Test]
        public void Test_Constructor_Default()
        {
            _rules = new InterruptionRules();

            Assert.IsTrue(_rules.Initiation.IsEmpty());
            Assert.IsTrue(_rules.Response.IsEmpty());
        }
        [Test]
        public void Test_Constructor()
        {
            Assert.AreEqual(INITIATION, _rules.Initiation);
            Assert.AreEqual(RESPONSE, _rules.Response);
        }
    }
}
