using Moq;
using NUnit.Framework;
using rharel.M3PD.Agency.Dialogue_Moves;
using rharel.M3PD.Common.Collections;
using System;

namespace rharel.M3PD.Agency.Modules.Tests
{
    [TestFixture]
    public sealed class RecentActivityTest
    {
        private static readonly string ALICE_ID = "alice";
        private static readonly DialogueMove GREETING = (
            new Mock<DialogueMove>().Object
        );
        private static readonly DialogueEvent GREETING_BY_ALICE = (
            new DialogueEvent(ALICE_ID, GREETING)
        );

        private RecentActivity _report;

        [SetUp]
        public void Setup()
        {
            _report = new RecentActivity();
        }

        [Test]
        public void Test_InitialState()
        {
            Assert.IsTrue(_report.Events.IsEmpty());
        }

        [Test]
        public void Test_Add_Indirect_WithInvalidArgs()
        {
            Assert.Throws<ArgumentNullException>(
                () => _report.Add(null, GREETING)
            );
            Assert.Throws<ArgumentNullException>(
                () => _report.Add(ALICE_ID, null)
            );
        }
        [Test]
        public void Test_Add_Indirect_WithIdleMove()
        {
            bool success = _report.Add(ALICE_ID, Idle.Instance);

            Assert.IsFalse(success);
        }
        [Test]
        public void Test_Add_Indirect_ExistingEvent()
        {
            _report.Add(ALICE_ID, GREETING);
            bool success = _report.Add(ALICE_ID, GREETING);

            Assert.IsFalse(success);
            Assert.AreEqual(1, _report.Events.Count);
        }
        [Test]
        public void Test_Add_Indirect()
        {
            bool success = _report.Add(ALICE_ID, GREETING);

            Assert.IsTrue(success);
            Assert.AreEqual(1, _report.Events.Count);
            Assert.IsTrue(_report.Events.Contains(GREETING_BY_ALICE));
        }

        [Test]
        public void Test_Add_WithIdleMove()
        {
            var @event = new DialogueEvent(ALICE_ID, Idle.Instance);
            bool success = _report.Add(@event);

            Assert.IsFalse(success);
        }
        [Test]
        public void Test_Add_ExistingEvent()
        {
            _report.Add(GREETING_BY_ALICE);
            bool success = _report.Add(GREETING_BY_ALICE);

            Assert.IsFalse(success);
            Assert.AreEqual(1, _report.Events.Count);
        }
        [Test]
        public void Test_Add()
        {
            bool success = _report.Add(GREETING_BY_ALICE);

            Assert.IsTrue(success);
            Assert.AreEqual(1, _report.Events.Count);
            Assert.IsTrue(_report.Events.Contains(GREETING_BY_ALICE));
        }
    }
}
