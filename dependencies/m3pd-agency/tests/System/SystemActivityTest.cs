using Moq;
using NUnit.Framework;
using rharel.Functional;
using rharel.M3PD.Agency.Dialogue_Moves;
using System;

namespace rharel.M3PD.Agency.System.Tests
{
    [TestFixture]
    public sealed class SystemActivityTest
    {
        private static readonly Optional<DialogueMove> RECENT_MOVE = (
            new Some<DialogueMove>(new Mock<DialogueMove>().Object)
        );
        private static readonly DialogueMove TARGET_MOVE = (
            new Mock<DialogueMove>().Object
        );
        private static readonly bool IS_ACTIVE = true;

        private SystemActivity _system_activity;

        [SetUp]
        public void Setup()
        {
            _system_activity = new SystemActivity(
                RECENT_MOVE, TARGET_MOVE, IS_ACTIVE
            );
        }

        [Test]
        public void Test_Construcor_WithInvalidArgs()
        {
            Assert.Throws<ArgumentNullException>(
                () => new SystemActivity(null, TARGET_MOVE, IS_ACTIVE)
            );
            Assert.Throws<ArgumentNullException>(
                () => new SystemActivity(RECENT_MOVE, null, IS_ACTIVE)
            );
        }
        [Test]
        public void Test_Constructor()
        {
            Assert.AreEqual(RECENT_MOVE, _system_activity.RecentMove);
            Assert.AreEqual(TARGET_MOVE, _system_activity.TargetMove);
            Assert.AreEqual(IS_ACTIVE, _system_activity.IsActive);
        }

        [Test]
        public void Test_Equality()
        {
            var original = _system_activity;
            var good_copy = new SystemActivity(
                original.RecentMove, 
                original.TargetMove,
                original.IsActive
            );
            var flawed_recent_move_copy = new SystemActivity(
                new None<DialogueMove>(),
                original.TargetMove,
                original.IsActive
            );
            var flawed_target_move_copy = new SystemActivity(
                original.RecentMove,
                new Mock<DialogueMove>().Object,
                original.IsActive
            );
            var flawed_is_active_copy = new SystemActivity(
                original.RecentMove,
                original.TargetMove,
                !IS_ACTIVE
            );

            Assert.AreNotEqual(original, null);
            Assert.AreNotEqual(original, "incompatible type");
            Assert.AreNotEqual(original, flawed_recent_move_copy);
            Assert.AreNotEqual(original, flawed_target_move_copy);
            Assert.AreNotEqual(original, flawed_is_active_copy);

            Assert.AreEqual(original, original);
            Assert.AreEqual(original, good_copy);
        }
    }
}
