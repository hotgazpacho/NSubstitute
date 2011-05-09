using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using NUnit.Framework;

namespace NSubstitute.Acceptance.Specs.FieldReports
{
    public class Issue48_SerialisingSubs
    {
        public interface IRoot { string Data { get; set; } }
        [Serializable] public abstract class Root { public abstract string Data { get; set; } };

        [Test]
        [Pending]
        public void SerialiseFromInterface()
        {
            var root = Substitute.For<IRoot>();
            var formatter = new BinaryFormatter();

            using (var stream = new MemoryStream())
            {
                formatter.Serialize(stream, root);
                stream.Position = 0;
                var newRoot = formatter.Deserialize(stream) as IRoot;
            }
        }

        [Test]
        [Pending]
        public void SerialiseFromClass()
        {
            var root = Substitute.For<Root>();
            var formatter = new BinaryFormatter();

            using (var stream = new MemoryStream())
            {
                formatter.Serialize(stream, root);
                stream.Position = 0;
                var newRoot = formatter.Deserialize(stream) as IRoot;
            }
        }

        [Test]
        [Pending]
        public void UseDeserialisedClass()
        {
            var root = Substitute.For<IRoot>();
            var formatter = new BinaryFormatter();

            using (var stream = new MemoryStream())
            {
                formatter.Serialize(stream, root);
                stream.Position = 0;
                var newRoot = (IRoot) formatter.Deserialize(stream);
                newRoot.Data = "hello";
                Assert.That(newRoot.Data, Is.EqualTo("hello"));
            }
        }
    }
}