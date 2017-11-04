using Moq;
using NUnit.Framework;
using rharel.Functional;
using rharel.M3PD.Agency.Dialogue_Moves;
using rharel.M3PD.Agency.Modules;
using System;
using System.Collections.Generic;
using System.Linq;

namespace rharel.M3PD.Expectations.Arrangement.Tests
{
    [TestFixture]
    public sealed class NodeTest
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

        private MockNode _node;

        [SetUp]
        public void Setup()
        {
            foreach (var child in CHILDREN) { child.Mock.Reset(); }

            _node = new MockNode(ID, CHILDREN);
        }

        [Test]
        public void Test_Constructor_WithInvalidArgs()
        {
            Assert.Throws<ArgumentNullException>(
                () => new MockNode(null)
            );
            Assert.Throws<ArgumentNullException>(
                () => new MockNode(null, CHILDREN)
            );
            Assert.Throws<ArgumentNullException>(
                () => new MockNode(ID, null)
            );
            Assert.Throws<ArgumentException>(
                () => new MockNode(" ", null)
            );
        }
        [Test]
        public void Test_Constructor()
        {
            Assert.AreEqual(ID, _node.ID);
            Assert.IsTrue(_node.Children.SequenceEqual(CHILDREN));
            Assert.AreEqual(Resolution.Pending, _node.Resolution);
            Assert.IsFalse(_node.IsResolved);
        }

        [Test]
        public void Test_Reset()
        {
            _node.Satisfy();

            _node.Reset();
            _node.Mock.Verify(x => x.OnReset(), Times.Once);
            Assert.AreEqual(Resolution.Pending, _node.Resolution);            
        }

        [Test]
        public void Test_Fail()
        {
            bool invoked_failed = false, 
                 invoked_resolved = false;

            _node.Resolved += sender =>
            {
                Assert.AreSame(_node, sender);
                Assert.IsFalse(invoked_resolved);
                Assert.IsFalse(invoked_failed);
                invoked_resolved = true;
            };
            _node.Failed += sender =>
            {
                Assert.AreSame(_node, sender);
                Assert.IsTrue(invoked_resolved);
                Assert.IsFalse(invoked_failed);
                invoked_failed = true;
            };
            _node.Fail();

            Assert.IsTrue(invoked_failed);
            Assert.IsTrue(invoked_resolved);
        }
        [Test]
        public void Test_Satisfy()
        {
            bool invoked_satisfied = false,
                 invoked_resolved = false;

            _node.Resolved += sender =>
            {
                Assert.AreSame(_node, sender);
                Assert.IsFalse(invoked_resolved);
                Assert.IsFalse(invoked_satisfied);
                invoked_resolved = true;
            };
            _node.Satisfied += sender =>
            {
                Assert.AreSame(_node, sender);
                Assert.IsTrue(invoked_resolved);
                Assert.IsFalse(invoked_satisfied);
                invoked_satisfied = true;
            };
            _node.Satisfy();

            Assert.IsTrue(invoked_satisfied);
            Assert.IsTrue(invoked_resolved);
        }

        [Test]
        public void Test_Process_AlreadyResolved()
        {
            _node.Satisfy();

            Assert.AreEqual(Resolution.Satisfaction, _node.Process(EVENT));

            _node.Mock.Verify(
                x => x.OnProcess(It.IsAny<DialogueEvent>()), 
                Times.Never
            );
            Assert.AreEqual(Resolution.Satisfaction, _node.Resolution);
        }
        [Test]
        public void Test_Process_Pending()
        {
            Assert.AreEqual(Resolution.Pending, _node.Process(EVENT));

            _node.Mock.Verify(x => x.OnProcess(EVENT), Times.Once);
            Assert.AreEqual(Resolution.Pending, _node.Resolution);
        }
        [Test]
        public void Test_Process_Failure()
        {
            _node.Mock.Setup(x => x.OnProcess(EVENT))
                      .Returns(Resolution.Failure);

            Assert.AreEqual(Resolution.Failure, _node.Process(EVENT));

            _node.Mock.Verify(x => x.OnProcess(EVENT), Times.Once);
            Assert.AreEqual(Resolution.Failure, _node.Resolution);
        }
        [Test]
        public void Test_Process_Satisfaction()
        {
            _node.Mock.Setup(x => x.OnProcess(EVENT))
                      .Returns(Resolution.Satisfaction);

            Assert.AreEqual(Resolution.Satisfaction, _node.Process(EVENT));

            _node.Mock.Verify(x => x.OnProcess(EVENT), Times.Once);
            Assert.AreEqual(Resolution.Satisfaction, _node.Resolution);
        }

        [Test]
        public void Test_GetScopeCarrierChain_WithAbsentCarrier()
        {
            _node.Mock.SetupGet(x => x.ScopeCarrierIndex)
                      .Returns(new None<int>());

            var chain = new List<Node>();
            _node.GetScopeCarrierChain(chain);

            Assert.IsTrue(new Node[] { _node }.SequenceEqual(chain));
        }
    }
}
