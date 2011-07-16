using System.Linq;
using NSubstitute.Core.Arguments;
using NUnit.Framework;

namespace NSubstitute.Specs.Arguments
{
    public class ArgumentMatcherSpecs
    {
        const int theAnswer = 42;
        const string theName = "Ultimate Answer to the Ultimate Question of Life, The Universe, and Everything";

        class Foo
        {
            public int Answer { get; set; }
            public string Name { get; set; }
        }

        class FooMatcher : ArgumentMatcher<Foo>
        {
            protected override void Evaluate(Foo value)
            {
                if(value.Answer != theAnswer) Mismatch(() => value.Answer, theAnswer);
                if(value.Name != theName)
                    Mismatch(() => value.Name, theName);
            }
        }

        [Test]
        public void Should_return_true_when_there_are_no_member_mismatches()
        {
            var foo = new Foo() {Answer = theAnswer, Name = theName};
            var fooMatcher = new FooMatcher();
            var result = fooMatcher.Match(foo);
            Assert.True(result);
        }

        [Test]
        public void Should_return_false_when_there_are_mismatched_members()
        {
            var foo = new Foo { Answer = 1, Name = "Name" };
            var fooMatcher = new FooMatcher();
            var result = fooMatcher.Match(foo);
            Assert.False(result);
        }

        [Test]
        public void Should_have_an_ArgumentMismatch_when_the_Answer_is_incorrect()
        {
            var foo = new Foo {Answer = 1, Name = "Name"};
            var fooMatcher = new FooMatcher();
            fooMatcher.Match(foo);
            CollectionAssert.Contains(fooMatcher.Mismatches, new MemberMismatch("Answer", 42, 1));
        }

        [Test]
        public void Should_have_an_ArgumentMismatch_when_the_Name_is_incorrect()
        {
            var foo = new Foo { Answer = 1, Name = "Name" };
            var fooMatcher = new FooMatcher();
            fooMatcher.Match(foo);
            CollectionAssert.Contains(fooMatcher.Mismatches, new MemberMismatch("Answer", "Ultimate Answer to the Ultimate Question of Life, The Universe, and Everything", "Name"));
        }
    }
}