using System.Linq;

namespace NSubstitute
{
    public class CallHandler : ICallHandler
    {
        readonly ICallStack _callStack;
        readonly ICallResults _callResults;
        readonly IPropertyHelper _propertyHelper;
        readonly ISubstitutionContext _context;
        bool _assertNextCallReceived;

        public CallHandler(ICallStack CallStack, ICallResults callResults, IPropertyHelper PropertyHelper, ISubstitutionContext context)
        {
            _callStack = CallStack;
            _callResults = callResults;
            _propertyHelper = PropertyHelper;
            _context = context;
        }

        public void LastCallShouldReturn<T>(T valueToReturn)
        {
            var lastCall = _callStack.Pop();
            _callResults.SetResult(lastCall, valueToReturn);
        }

        public object Handle(ICall call)
        {
            if (_assertNextCallReceived)
            {
                _assertNextCallReceived = false;
                _callStack.ThrowIfCallNotFound(call);
                return _callResults.GetDefaultResultFor(call);
            }
            if (_propertyHelper.IsCallToSetAReadWriteProperty(call))
            {
                var callToPropertyGetter = _propertyHelper.CreateCallToPropertyGetterFromSetterCall(call);
                var valueBeingSetOnProperty = call.GetArguments().First();
                _callResults.SetResult(callToPropertyGetter, valueBeingSetOnProperty);
            }
            _callStack.Push(call);
            _context.LastCallHandler(this);
            return _callResults.GetResult(call);
        }

        public void AssertNextCallHasBeenReceived()
        {
            _assertNextCallReceived = true;
        }
    }
}