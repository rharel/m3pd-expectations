using Moq;
using NUnit.Framework;
using rharel.M3PD.Agency.Dialogue_Moves;
using System;

namespace rharel.M3PD.Agency.Modules.Tests
{
    [TestFixture]
    public sealed class DialogueEventTest
    {
        private static readonly string SOURCE_ID = "mock id";
        private static readonly DialogueMove MOVE = (
            new Mock<DialogueMove>().Object
        );

        private DialogueEvent _event;

        [SetUp]
        public void Setup()
        {
            _event = new DialogueEvent(SOURCE_ID, MOVE);
        }

        [Test]
        public void Test_Construcor_WithInvalidArgs()
        {
            Assert.Throws<ArgumentNullException>(
                () => new DialogueEvent(null, MOVE)
            );
            Assert.Throws<ArgumentNullException>(
                () => new DialogueEvent(SOURCE_ID, null)
            );
        }
        [Test]
        public void Test_Constructor()
        {
            Assert.AreEqual(SOURCE_ID, _event.SourceID);
            Assert.AreEqual(MOVE, _event.Move);
        }

        [Test]
        public void Test_Equality()
        {
            var original = _event;
            var good_copy = new DialogueEvent(
                original.SourceID, 
                original.Move
            );
            var flawed_source_copy = new DialogueEvent(
                $"wrong {SOURCE_ID}",
                original.Move
            );
            var flawed_move_copy = new DialogueEvent
            (
                original.SourceID,
                new Mock<DialogueMove>().Object
            );

            Assert.AreNotEqual(original, null);
            Assert.AreNotEqual(original, "incompatible type");
            Assert.AreNotEqual(original, flawed_source_copy);
            Assert.AreNotEqual(original, flawed_move_copy);

            Assert.AreEqual(original, original);
            Assert.AreEqual(original, good_copy);
        }
    }
}
