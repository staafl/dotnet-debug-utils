using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;

// Assembly.LoadFrom("\\VELKO-PC\share\debug-utils.dll");

// todo:
// - filesystemwatcher
public static class DebugHelpers
{
    public static void Main()
    {
        Console.WriteLine("System.Reflection.Assembly.LoadFrom(@\"" + Assembly.GetExecutingAssembly().Location + "\")");
    }
    public static void BreakOnFileAccess(string path) 
    {
        var msg = Path.GetFullPath(path);
        msg = "The process cannot access the file '" + msg;
        msg = msg.ToUpper();
        
        var fs = File.Open(path, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
        new Thread(() => {
            while (true) 
            {   
                Thread.Sleep(Timeout.Infinite);
                GC.KeepAlive(fs);
            }
        }).Start();
        
        AppDomain.CurrentDomain.FirstChanceException += (sender, e) => {
            if (e.Exception is IOException && 
                e.Exception.Message.ToUpper().Contains(msg))
            {
                Debugger.Break();
            }
        };
    }
    
    public static void BreakOnEvent(object obj, string name)
    {
        var evt = obj.GetType().GetEvent(name);
        var field = obj.GetType().GetField(
            evt.Name, 
            BindingFlags.NonPublic |
            BindingFlags.Instance |
            BindingFlags.GetField);

        var dlg = (System.EventHandler)field.GetValue(obj);
        
#warning LATER
// other kinds of event handlers
        field.SetValue(obj, (EventHandler)((sender, ea) => { Debugger.Break(); dlg.Invoke(sender, ea); }));
	
    }
}

#warning LATER
// use HRESULT
public static class Strinq
{
    public static IEnumerable<object> Select<TSource>(this IEnumerable<TSource> seq, string property)
    {
        var prop = typeof(TSource).GetProperty(property);
        return seq.Select(s => prop.GetValue(s));
    }
   
    public static IEnumerable<TSource> Where<TSource>(this IEnumerable<TSource> seq, string property, TSource value)
    {
        var prop = typeof(TSource).GetProperty(property);
        return seq.Where(s => Object.Equals(s, (TSource)prop.GetValue(s)));
    }
}

