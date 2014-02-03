using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Permissions;
using System.Text;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;

  
public class DebugProxy<T> : RealProxy
{
  readonly T inner;
  
  public DebugProxy(T inner)
      : base(typeof(T))
  {
    if (inner == null)
        throw new ArgumentNullException("inner");
    this.inner = inner;
  }
  
  public event Action<string> MethodInvoked;

  // [SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
  public override IMessage Invoke(IMessage msg)
  {
    if (msg == null)
        throw new ArgumentNullException("msg");
        
    var methodCallMessage = msg as IMethodCallMessage;

    if (methodCallMessage == null)
        throw new ArgumentException("msg as IMethodCallMessage == null");
        
    ReturnMessage responseMessage;
    Object response = null;
    Exception caughtException = null;

    try
    {
        String methodName = (String)msg.Properties["__MethodName"];
        Type[] parameterTypes = (Type[])msg.Properties["__MethodSignature"];
        var method = typeof(T).GetMethod(methodName, parameterTypes);

        object[] parameters = (object[])msg.Properties["__Args"];
        
        var invoked = MethodInvoked;
        if (invoked != null)
          invoked(methodName);
        
        response = method.Invoke(inner, parameters);
    }
    catch (Exception ex)
    {
        caughtException = ex;
    }


    if (caughtException == null)
        responseMessage = new ReturnMessage(response, null, 0, null, methodCallMessage);
    else
        responseMessage = new ReturnMessage(caughtException, methodCallMessage);

    return responseMessage;
  }
}