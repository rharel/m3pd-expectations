using NUnit.Framework;
using rharel.M3PD.Expectations.Arrangement.Tests;
using System;

namespace rharel.M3PD.Expectations.State.Tests
{
    [TestFixture]
    public sealed class SocialContextTest
    {
        private static readonly string ID = "agent id";
        private static readonly MockNode EXPECTATIONS = (
            new MockNode("node id")
        );

        private SocialContext _context;

        [SetUp]
        public void Setup()
        {
            _context = new SocialContext(ID, EXPECTATIONS);
        }
        [Test]
        public void Test_Constructor_WithInvalidArgs()
        {
            Assert.Throws<ArgumentNullException>(
                () => new SocialContext(null, EXPECTATIONS)
            );
            Assert.Throws<ArgumentException>(
                () => new SocialContext(" ", EXPECTATIONS)
            );
            Assert.Throws<ArgumentNullException>(
                () => new SocialContext(ID, null)
            );
        }
        [Test]
        public void Test_Constructor()
        {
            Assert.AreEqual(ID, _context.SelfID);
            Assert.AreSame(EXPECTATIONS, _context.Interaction);
        }

        [Test]
        public void Test_Equality()
        {
            var original = new SocialContext(ID, EXPECTATIONS);
            var good_copy = new SocialContext(ID, EXPECTATIONS);
            var flawed_agent_copy = new SocialContext(
                $"other {ID}", 
                EXPECTATIONS
            );
            var flawed_expectations_copy = new SocialContext(
                ID,
                new MockNode("other expectations id")
            );

            Assert.AreNotEqual(original, null);
            Assert.AreNotEqual(original, "incompatible type");
            Assert.AreNotEqual(original, flawed_agent_copy);
            Assert.AreNotEqual(original, flawed_expectations_copy);

            Assert.AreEqual(original, original);
            Assert.AreEqual(original, good_copy);
        }
    }
}
