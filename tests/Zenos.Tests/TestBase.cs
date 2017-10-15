

using System;
using Xunit;
using Zenos.Framework;

namespace Zenos.Tests
{
    public class TestBase
    {
                    
        protected void Fchar(CharDelegate func, Action<IMethodContext> action = null) 
        {
            F<CharDelegate, char>(f => f(), action)(func);
        }
                    
        protected void Fbool(BoolDelegate func, Action<IMethodContext> action = null) 
        {
            F<BoolDelegate, bool>(f => f(), action)(func);
        }
                    
        protected void Fuint(UInt32Delegate func, Action<IMethodContext> action = null) 
        {
            F<UInt32Delegate, uint>(f => f(), action)(func);
        }
                    
        protected void Fint(Int32Delegate func, Action<IMethodContext> action = null) 
        {
            F<Int32Delegate, int>(f => f(), action)(func);
        }
                    
        protected void Fulong(UInt64Delegate func, Action<IMethodContext> action = null) 
        {
            F<UInt64Delegate, ulong>(f => f(), action)(func);
        }
                    
        protected void Flong(Int64Delegate func, Action<IMethodContext> action = null) 
        {
            F<Int64Delegate, long>(f => f(), action)(func);
        }
                    
        protected void Ffloat(SingleDelegate func, Action<IMethodContext> action = null) 
        {
            F<SingleDelegate, float>(f => f(), action)(func);
        }
                    
        protected void Fdouble(DoubleDelegate func, Action<IMethodContext> action = null) 
        {
            F<DoubleDelegate, double>(f => f(), action)(func);
        }

        protected Action<TDelegate> F<TDelegate, TResult>(Func<TDelegate, TResult> func, Action<IMethodContext> action = null) 
            where TDelegate : class
        {            
            return Test.Runs(func, Assert.Equal, action);
        }
    }
}


