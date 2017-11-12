using Moq;
using rharel.Functional;
using rharel.M3PD.Agency.Modules;
using System.Collections.Generic;
using static rharel.Functional.Option;

namespace rharel.M3PD.Expectations.Arrangement.Tests
{
    public interface IMockNode
    {
        Optional<int> ScopeCarrierIndex { get; }
        void OnReset();
        Resolution OnProcess(DialogueEvent @event);
        void GetExpectedEvents(ICollection<DialogueEvent> result);
    }
    internal sealed class MockNode: Node
    {
        
        public Mock<IMockNode> Mock { get; } = new Mock<IMockNode>();

        public MockNode(string id): base(id)
        {
            ResetMock();
        }
        public MockNode(string id, IEnumerable<Node> children)
            : base(id, children)
        {
            ResetMock();
        }

        public override Optional<int> ScopeCarrierIndex => (
            Mock.Object.ScopeCarrierIndex
        );

        protected override void OnReset()
        {
            Mock.Object.OnReset();
        }
        protected override Resolution OnProcess(DialogueEvent @event)
        {
            return Mock.Object.OnProcess(@event);
        }

        public override void GetExpectedEvents(
            ICollection<DialogueEvent> result)
        {
            Mock.Object.GetExpectedEvents(result);
        }

        public void ResetMock()
        {
            Mock.Reset();
            Mock.SetupGet(x => x.ScopeCarrierIndex)
                .Returns(None<int>());
            Mock.Setup(x => x.OnProcess(It.IsAny<DialogueEvent>()))
                .Returns(Resolution.Pending);
        }
    }
}
