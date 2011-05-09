using System;
using Castle.DynamicProxy;
using NSubstitute.Core;

namespace NSubstitute.Proxies.CastleDynamicProxy
{
    [Serializable]
    public class CastleForwardingInterceptor : IInterceptor
    {
        [NonSerialized]
        readonly CastleInvocationMapper _invocationMapper;

        [NonSerialized]
        readonly ICallRouter _callRouter;

        public CastleForwardingInterceptor(CastleInvocationMapper invocationMapper, ICallRouter callRouter)
        {
            _invocationMapper = invocationMapper;
            _callRouter = callRouter;
        }

        public void Intercept(IInvocation invocation)
        {
            var mappedInvocation = _invocationMapper.Map(invocation);
            invocation.ReturnValue = _callRouter.Route(mappedInvocation);
        }
    }
}