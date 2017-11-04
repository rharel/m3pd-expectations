using NUnit.Framework;
using rharel.Functional;

namespace rharel.M3PD.Agency.Dialogue_Moves.Tests
{
    [TestFixture]
    public sealed class IdleTest
    {
        [Test]
        public void Test_Instance()
        {
            Assert.IsNotNull(Idle.Instance);
            Assert.AreEqual("idle", Idle.Instance.Type);
            Assert.IsTrue(Idle.Instance.Properties.IsNone());
        }
    }
}
