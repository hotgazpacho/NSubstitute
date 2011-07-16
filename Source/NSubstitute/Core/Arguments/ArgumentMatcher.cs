using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace NSubstitute.Core.Arguments
{
    public interface IArgumentMatcher<TValue>
    {
        bool Match(TValue value);
        IEnumerable<MemberMismatch> Mismatches { get; } 
    }

    public abstract class ArgumentMatcher<TValue> : IArgumentMatcher<TValue>
    {
        public bool Match(TValue value)
        {
            Evaluate(value);
            return _mismatches.Any() == false;
        }

        public IEnumerable<MemberMismatch> Mismatches { get { return _mismatches; } } 

        protected void Mismatch<T>(Expression<Func<T>> expression, object expected)
        {
            var member = (MemberExpression)expression.Body;
            var memberName = member.Member.Name;
            T actual = expression.Compile()();

            var mismatch = new MemberMismatch(memberName, actual, expected);
            _mismatches.Add(mismatch);
        }

        protected abstract void Evaluate(TValue value);

        readonly ICollection<MemberMismatch> _mismatches = new List<MemberMismatch>();
    }
}