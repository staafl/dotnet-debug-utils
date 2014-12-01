using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;

public static class DebugHelpers
{
    public static object Eval(this object obj, string path)
    {
      string soFar = "<object>";
      const BindingFlags FLAGS = 
          BindingFlags.Instance |
          BindingFlags.NonPublic | 
          BindingFlags.Public | 
          BindingFlags.IgnoreCase |
          BindingFlags.FlattenHierarchy;
         
      foreach (string elem in path.Split('.'))
      {
          if (obj == null)
          {
            return soFar + " = <null>";
          }
          soFar += "." + elem;
          FieldInfo field = obj.GetType().GetField(elem, FLAGS);
          if (field != null)
          {
            obj = field.GetValue(obj);
            continue;
          }
          PropertyInfo prop = obj.GetType().GetProperty(elem, FLAGS);
          if (prop != null)
          {
            obj = prop.GetValue(obj);
            continue;
          }
          
          return soFar + " not found";
      }
      return obj ?? (object)"<null>";
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

        bool doBreak = true;
        field.SetValue(obj, (EventHandler)((sender, ea) => { if (doBreak) Debugger.Break(); dlg.Invoke(sender, ea); }));
	
    }
}