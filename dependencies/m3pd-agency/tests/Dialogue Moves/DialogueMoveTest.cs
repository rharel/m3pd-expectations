using NUnit.Framework;
using rharel.Functional;
using System;

namespace rharel.M3PD.Agency.Dialogue_Moves.Tests
{
    [TestFixture]
    public sealed class DialogueMoveTest
    {
        private class Foo { }
        private sealed class FooDerived: Foo { }
        private sealed class Bar { }

        private static readonly string TYPE = "mock_type";
        private static readonly string PROPERTIES = "mock_properties";

        [Test]
        public void Test_Constructor_WithInvalidArgs()
        {
            Assert.Throws<ArgumentNullException>(
                () => new DialogueMove<string>(null, PROPERTIES)
            );
            Assert.Throws<ArgumentException>(
                () => new DialogueMove<string>(" ", PROPERTIES)
            );
            Assert.Throws<ArgumentNullException>(
                () => new DialogueMove<string>(TYPE, null)
            );
        }
        [Test]
        public void Test_Constructor_WithoutProperties()
        {
            var move = new DialogueMove<string>(TYPE);

            Assert.AreEqual(TYPE, move.Type);
            Assert.IsTrue(move.Properties.IsNone());
        }
        [Test]
        public void Test_Constructor_WithProperties()
        { 
            var move = new DialogueMove<string>(TYPE, PROPERTIES);

            Assert.AreEqual(TYPE, move.Type);
            Assert.IsTrue(move.Properties.Contains(PROPERTIES));
        }

        [Test]
        public void Test_Cast_ToInvalidType()
        {
            Assert.Throws<InvalidCastException>(
                () => new DialogueMove<Foo>(TYPE, new Foo()).Cast<Bar>()
            );
        }
        [Test]
        public void Test_Cast_Up()
        {
            var source = new DialogueMove<FooDerived>(TYPE, new FooDerived());
            var target = source.Cast<Foo>();

            Assert.AreEqual(TYPE, target.Type);
            Assert.AreEqual(target.Properties, source.Properties);
        }
        [Test]
        public void Test_Cast_Down()
        {
            var source = new DialogueMove<Foo>(TYPE, new FooDerived());
            var target = source.Cast<FooDerived>();

            Assert.AreEqual(TYPE, target.Type);
            Assert.AreEqual(target.Properties, source.Properties);
        }

        [Test]
        public void Test_Equality()
        {
            var original = new DialogueMove<string>(TYPE, PROPERTIES);
            var good_copy = new DialogueMove<string>(
                original.Type, original.Properties.Unwrap()
            );
            var flawed_type_copy = (
                new DialogueMove<string>(
                    $"other {original.Type}", 
                    original.Properties.Unwrap()
                )
            );
            var flawed_properties_copy = (
                new DialogueMove<string>(
                    original.Type,
                    $"other {original.Properties.Unwrap()}"
                )
            );

            Assert.AreNotEqual(original, null);
            Assert.AreNotEqual(original, "incompatible type");
            Assert.AreNotEqual(original, flawed_type_copy);
            Assert.AreNotEqual(original, flawed_properties_copy);

            Assert.AreEqual(original, original);
            Assert.AreEqual(original, good_copy);
        }
        [Test]
        public void Test_Equality_WhenCast()
        {
            Assert.AreEqual(
                new DialogueMove<object>(TYPE, PROPERTIES),
                new DialogueMove<string>(TYPE, PROPERTIES)
            );
        }
    }
}
