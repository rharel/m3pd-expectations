using Moq;
using NUnit.Framework;
using rharel.M3PD.Agency.Dialogue_Moves;
using rharel.M3PD.Agency.Modules;
using System;
using System.Collections.Generic;

namespace rharel.M3PD.Expectations.Arrangement.Tests
{
    [TestFixture]
    public sealed class RepetitionTest
    {
        private static readonly string ID = "event id";
        private static readonly MockNode BODY = new MockNode("body");
        private static readonly DialogueEvent EVENT = (
            new DialogueEvent("source id", new Mock<DialogueMove>().Object)
        );

        private Repetition _node;

        [SetUp]
        public void Setup()
        {
            BODY.Reset();
            BODY.ResetMock();

            _node = new Repetition(ID, BODY);
        }

        [Test]
        public void Test_Constructor_WithInvalidArgs()
        {
            Assert.Throws<ArgumentNullException>(
                () => new Repetition(ID, null)
            );
        }

        [Test]
        public void Test_Process_Pending()
        {
            _node.Process(EVENT);

            Assert.AreEqual(Resolution.Pending, _node.Resolution);
        }
        [Test]
        public void Test_Process_Failure()
        {
            BODY.Mock.Setup(x => x.OnProcess(EVENT))
                     .Returns(Resolution.Failure);

            _node.Process(EVENT);

            Assert.AreEqual(Resolution.Failure, _node.Resolution);
        }
        [Test]
        public void Test_Process_Satisfaction()
        {
            BODY.Mock.Setup(x => x.OnProcess(EVENT))
                     .Returns(Resolution.Satisfaction);

            _node.Process(EVENT);

            Assert.AreEqual(Resolution.Pending, _node.Resolution);
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

            BODY.Mock.Verify(
                x => x.GetExpectedEvents(list),
                Times.Once
            );
        }
    }
}
