using System;
/*
class Program
{
  interface IFoo
  {
    void Bar();
  }
  
  class Foo : IFoo
  {
    public void Bar() 
    {
      Console.WriteLine("Inside method");
    }
  }
  
  static void Main(string[] args) 
  {
      var proxy = new DebugProxy<IFoo>(new Foo());
      IFoo fooProxy = (IFoo)proxy.GetTransparentProxy();
      proxy.MethodInvoked += name => Console.WriteLine("{0} invoked", name);
      fooProxy.Bar();
  }
}
*/