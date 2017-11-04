using Moq;
using NUnit.Framework;
using rharel.M3PD.Agency.Dialogue_Moves;
using rharel.M3PD.Agency.State;

namespace rharel.M3PD.Agency.Modules.Tests
{
    [TestFixture]
    public sealed class ActionRealizationModuleStubTest
    {
        private static readonly StateAccessor STATE = (
            new Mock<StateAccessor>().Object
        );
        private static readonly DialogueMove MOVE = (
            new Mock<DialogueMove>().Object
        );

        private ARStub _stub;

        [SetUp]
        public void Setup()
        {
            _stub = new ARStub();
            _stub.Initialize(STATE);
        }

        [Test]
        public void Test_RealizeMove()
        {
            Assert.AreEqual(
                RealizationStatus.Complete, 
                _stub.RealizeMove(MOVE)
            );
        }
    }
}
