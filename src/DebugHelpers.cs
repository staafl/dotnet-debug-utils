using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;

public static class DebugHelpers
{
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

        #warning LATER
        // other kinds of event handler delegate types
        var dlg = (System.EventHandler)field.GetValue(obj);
        

        bool doBreak = true;
        field.SetValue(obj, (EventHandler)((sender, ea) => { if (doBreak) Debugger.Break(); dlg.Invoke(sender, ea); }));
	
    }
}