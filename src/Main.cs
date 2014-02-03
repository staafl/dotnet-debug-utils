using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;
class Program
{
    public static void Main()
    {
        Console.WriteLine("System.Reflection.Assembly.LoadFrom(@\"" + Assembly.GetExecutingAssembly().Location + "\")");
    }
}
  