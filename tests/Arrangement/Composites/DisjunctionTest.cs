using Moq;
using NUnit.Framework;
using rharel.M3PD.Agency.Dialogue_Moves;
using rharel.M3PD.Agency.Modules;
using System;
using System.Collections.Generic;

namespace rharel.M3PD.Expectations.Arrangement.Tests
{
    [TestFixture]
    public sealed class DisjunctionTest
    {
        private static readonly string ID = "event id";
        private static readonly MockNode[] CHILDREN = new MockNode[]
        {
            new MockNode("child 1"),
            new MockNode("child 2")
        };
        private static readonly DialogueEvent EVENT = (
            new DialogueEvent("source id", new Mock<DialogueMove>().Object)
        );

        private Disjunction _node;

        [SetUp]
        public void Setup()
        {
            foreach (var child in CHILDREN)
            {
                child.Reset();
                child.ResetMock();
            }
            _node = new Disjunction(ID, CHILDREN);
        }

        [Test]
        public void Test_Constructor_WithInvalidArgs()
        {
            Assert.Throws<ArgumentNullException>(
                () => new Disjunction(ID, null)
            );
            Assert.Throws<ArgumentException>(
                () => new Disjunction(ID, new Node[0])
            );
        }

        [Test]
        public void Test_Process_Pending()
        {
            CHILDREN[0].Mock.Setup(x => x.OnProcess(EVENT))
                            .Returns(Resolution.Failure);

            _node.Process(EVENT);

            Assert.AreEqual(Resolution.Pending, _node.Resolution);
        }
        [Test]
        public void Test_Process_Failure()
        {
            CHILDREN[0].Mock.Setup(x => x.OnProcess(EVENT))
                            .Returns(Resolution.Failure);
            CHILDREN[1].Mock.Setup(x => x.OnProcess(EVENT))
                            .Returns(Resolution.Failure);
            
            _node.Process(EVENT);

            Assert.AreEqual(Resolution.Failure, _node.Resolution);
        }
        [Test]
        public void Test_Process_Satisfaction()
        {
            CHILDREN[0].Mock.Setup(x => x.OnProcess(EVENT))
                            .Returns(Resolution.Satisfaction);

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

            CHILDREN[0].Mock.Verify(
                x => x.GetExpectedEvents(list),
                Times.Once
            );
            CHILDREN[1].Mock.Verify(
                x => x.GetExpectedEvents(list),
                Times.Once
            );
        }
    }
}
