using System;
using System.Diagnostics;
using System.IO;
using Castle.DynamicProxy;

namespace Bomberman.Logging
{
    public class LoggingAspect : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            try
            {
                invocation.Proceed();
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
                using (var writer = new StreamWriter("Exceptions.txt"))
                {
                    writer.Write(exception.Message);
                    writer.Write(exception.StackTrace);
                }
            }
        }
    }
}