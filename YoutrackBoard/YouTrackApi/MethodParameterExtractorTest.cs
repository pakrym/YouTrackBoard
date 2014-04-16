namespace YoutrackBoard
{
    using System;
    using System.Linq.Expressions;

    using NUnit.Framework;

    [TestFixture]
    public class MethodParameterExtractorTest
    {
        [Test]
        public void TestConstString()
        {
            Expression<Func<bool>> m = () => string.IsNullOrEmpty("Value");
            var p = MethodParameterExtractor.GetObjects(m);
            Assert.That(p, Has.Length.EqualTo(1));
            Assert.That(p, Contains.Item("Value"));
        }

        [Test]

        public void TestLocal()
        {
            object a = 1;
            object b = Guid.NewGuid();

            Expression<Action> m = () => this._TestParams(a, b);
            var p = MethodParameterExtractor.GetObjects(m);
            Assert.That(p, Has.Length.EqualTo(2));
            Assert.That(p, Contains.Item(a));
            Assert.That(p, Contains.Item(b));
        }

        [Test]
        [TestCase(1,2)]
        public void TestParameters(object a, object b)
        {
            Expression<Action> m = () => this._TestParams(a, b);
            var p = MethodParameterExtractor.GetObjects(m);
            Assert.That(p, Has.Length.EqualTo(2));
            Assert.That(p, Contains.Item(a));
            Assert.That(p, Contains.Item(b));
        }

        private void _TestParams(object a, object b)
        {
            
        }
    }
}