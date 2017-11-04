using NUnit.Framework;

namespace rharel.M3PD.Agency.Dialogue_Moves.Tests
{
    [TestFixture]
    public sealed class ExtensionsTest
    {
        [Test]
        public void Test_IsIdle()
        {
            Assert.IsTrue(Idle.Instance.IsIdle());
            Assert.IsTrue(new DialogueMove<object>("idle").IsIdle());
            Assert.IsFalse(new DialogueMove<object>("not-idle").IsIdle());
        }
    }
}
