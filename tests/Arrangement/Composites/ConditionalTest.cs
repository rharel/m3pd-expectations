using Moq;
using NUnit.Framework;
using rharel.M3PD.Agency.Dialogue_Moves;
using rharel.M3PD.Agency.Modules;
using rharel.M3PD.Common.Delegates;
using System;
using System.Collections.Generic;

namespace rharel.M3PD.Expectations.Arrangement.Tests
{
    [TestFixture]
    public sealed class ConditionalTest
    {
        private static readonly string ID = "event id";
        private static readonly MockNode BODY = new MockNode("body");
        private static readonly DialogueEvent EVENT = (
            new DialogueEvent("source id", new Mock<DialogueMove>().Object)
        );

        private bool _condition_value;

        private Conditional _node;

        [SetUp]
        public void Setup()
        {
            BODY.Reset();
            BODY.ResetMock();

            _condition_value = true;
            _node = new Conditional(ID, BODY, () => _condition_value);
        }

        [Test]
        public void Test_Constructor_WithInvalidArgs()
        {
            Assert.Throws<ArgumentNullException>(
                () => new Conditional(ID, null, Predicates.Always)
            );
            Assert.Throws<ArgumentNullException>(
                () => new Conditional(ID, BODY, null)
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

            Assert.AreEqual(Resolution.Satisfaction, _node.Resolution);
        }
        [Test]
        public void Test_Process_Condition()
        {
            _condition_value = false;
            _node.Process(EVENT);

            BODY.Mock.Verify(x => x.OnProcess(EVENT), Times.Never);

            _condition_value = true;
            _node.Process(EVENT);

            BODY.Mock.Verify(x => x.OnProcess(EVENT), Times.Once);
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
            
            _condition_value = false;
            _node.GetExpectedEvents(list);

            BODY.Mock.Verify(x => x.GetExpectedEvents(list), Times.Never);

            _condition_value = true;
            _node.GetExpectedEvents(list);

            BODY.Mock.Verify(x => x.GetExpectedEvents(list), Times.Once);
        }
    }
}
