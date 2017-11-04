using Moq;
using NUnit.Framework;
using rharel.M3PD.Agency.Dialogue_Moves;
using rharel.M3PD.Agency.State;

namespace rharel.M3PD.Agency.Modules.Tests
{
    [TestFixture]
    public sealed class ATStubTest
    {
        private static readonly StateAccessor STATE = (
            new Mock<StateAccessor>().Object
        );
        private static readonly DialogueMove MOVE = (
            new Mock<DialogueMove>().Object
        );

        private ATStub _stub;

        [SetUp]
        public void Setup()
        {
            _stub = new ATStub();
            _stub.Initialize(STATE);
        }

        [Test]
        public void Test_IsValidMoveNow()
        {
            Assert.IsTrue(_stub.IsValidMoveNow(MOVE));
        }
    }
}
