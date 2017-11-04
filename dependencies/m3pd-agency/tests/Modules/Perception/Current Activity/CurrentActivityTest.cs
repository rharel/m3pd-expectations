using NUnit.Framework;
using rharel.M3PD.Common.Collections;
using System;

namespace rharel.M3PD.Agency.Modules.Tests
{
    [TestFixture]
    public sealed class CurrentActivityTest
    {
        private static readonly string ALICE_ID = "alice";
        private static readonly string BOB_ID = "bob";

        private CurrentActivity _report;

        [SetUp]
        public void Setup()
        {
            _report = new CurrentActivity();
        }

        [Test]
        public void Test_InitialState()
        {
            Assert.IsTrue(_report.PassiveIDs.IsEmpty());
            Assert.IsTrue(_report.ActiveIDs.IsEmpty());
        }

        [Test]
        public void Test_Contains_WithInvalidArgs()
        {
            Assert.Throws<ArgumentNullException>(
                () => _report.Contains(null)
            );
        }
        [Test]
        public void Test_Contains()
        {
            _report.MarkPassive(ALICE_ID);
            _report.MarkActive(BOB_ID);

            Assert.IsTrue(_report.Contains(ALICE_ID));
            Assert.IsTrue(_report.Contains(BOB_ID));
            Assert.IsFalse(_report.Contains("charlie"));
        }

        [Test]
        public void Test_MarkPassive_WithInvalidArgs()
        {
            Assert.Throws<ArgumentNullException>(
                () => _report.MarkPassive(null)
            );
        }
        [Test]
        public void Test_MarkPassive_OfExistingPassiveAgent()
        {
            _report.MarkPassive(ALICE_ID);
            bool success = _report.MarkPassive(ALICE_ID);

            Assert.IsFalse(success);
        }
        [Test]
        public void Test_MarkPassive_OfExistingActiveAgent()
        {
            _report.MarkActive(ALICE_ID);
            bool success = _report.MarkPassive(ALICE_ID);

            Assert.IsFalse(success);
        }
        [Test]
        public void Test_MarkPassive()
        {
            bool success = _report.MarkPassive(ALICE_ID);

            Assert.IsTrue(success);
            Assert.AreEqual(1, _report.PassiveIDs.Count);
            Assert.IsTrue(_report.PassiveIDs.Contains(ALICE_ID));
        }

        [Test]
        public void Test_MarkActive_WithInvalidArgs()
        {
            Assert.Throws<ArgumentNullException>
            (
                () => _report.MarkActive(null)
            );
        }
        [Test]
        public void Test_MarkActive_OfExistingPassiveAgent()
        {
            _report.MarkPassive(ALICE_ID);
            bool success = _report.MarkActive(ALICE_ID);

            Assert.IsFalse(success);
        }
        [Test]
        public void Test_MarkActive_OfExistingActiveAgent()
        {
            _report.MarkActive(ALICE_ID);
            bool success = _report.MarkActive(ALICE_ID);

            Assert.IsFalse(success);
        }
        [Test]
        public void Test_MarkActive()
        {
            bool success = _report.MarkActive(ALICE_ID);

            Assert.IsTrue(success);
            Assert.AreEqual(1, _report.ActiveIDs.Count);
            Assert.IsTrue(_report.ActiveIDs.Contains(ALICE_ID));
        }

        [Test]
        public void Test_GetStatus_WithInvalidArgs()
        {
            Assert.Throws<ArgumentNullException>(
                () => _report.GetStatus(null)
            );
            Assert.Throws<ArgumentException>(
                () => _report.GetStatus(ALICE_ID)
            );
        }
        [Test]
        public void Test_GetStatus()
        {
            _report.MarkPassive(ALICE_ID);
            _report.MarkActive(BOB_ID);

            Assert.AreEqual(
                ActivityStatus.Passive, 
                _report.GetStatus(ALICE_ID)
            );
            Assert.AreEqual(
                ActivityStatus.Active,
                _report.GetStatus(BOB_ID)
            );
        }
    }
}
