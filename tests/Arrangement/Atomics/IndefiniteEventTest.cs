using Moq;
using NUnit.Framework;
using rharel.M3PD.Agency.Dialogue_Moves;
using rharel.M3PD.Agency.Modules;
using System;
using System.Collections.Generic;

namespace rharel.M3PD.Expectations.Arrangement.Tests
{
    [TestFixture]
    public sealed class IndefiniteEventTest
    {
        private static readonly string ID = "event id";
        private static readonly DialogueEvent EVENT = (
            new DialogueEvent("source id", new Mock<DialogueMove>().Object)
        );

        private IndefiniteEvent _node;

        [SetUp]
        public void Setup()
        {
            _node = new IndefiniteEvent(ID, EVENT);
        }

        [Test]
        public void Test_Constructor_WithInvalidArgs()
        {
            Assert.Throws<ArgumentNullException>(
                () => new IndefiniteEvent(null, EVENT)
            );
            Assert.Throws<ArgumentException>(
                () => new IndefiniteEvent(" ", EVENT)
            );
        }
        [Test]
        public void Test_Constructor()
        {
            Assert.AreEqual(ID, _node.ID);
            Assert.AreEqual(EVENT, _node.Event);
            Assert.AreEqual(Resolution.Pending, _node.Resolution);
            Assert.IsFalse(_node.IsResolved);
        }

        [Test]
        public void Test_Process()
        {
            _node.Process(EVENT);

            Assert.AreEqual(Resolution.Satisfaction, _node.Resolution);
        }

        [Test]
        public void Test_GetExpectedEvents_WithInvalidArgs()
        {
            Assert.Throws<ArgumentNullException>(
                () => _node.GetExpectedEvents(null)
            );
        }
        [Test]
        public void Test_GetExpectedEvents()
        {
            var list = new List<DialogueEvent>();
            _node.GetExpectedEvents(list);

            Assert.AreEqual(1, list.Count);
            Assert.IsTrue(list.Contains(_node.Event));
        }
    }
}
