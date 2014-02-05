using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

public static class Strinq
{
    public static object Prop(this object obj, string prop) 
    {
        return obj.GetType().GetProperty(prop).GetValue(obj);
    }
    
    public static void Dump<TSource>(this IEnumerable<TSource> seq)
    {
        var filePath = Path.GetTempFileName();
        using(var sw = new StreamWriter(filePath))
        {
            foreach(var elem in seq)
                sw.WriteLine(elem);
        }
        Process.Start(filePath);
    }
    
    public static IEnumerable<object> Select<TSource>(this IEnumerable<TSource> seq, string property)
    {
        var prop = typeof(TSource).GetProperty(property);
        return seq.Select(s => prop.GetValue(s));
    }
    
    public static IEnumerable<TSource> Where<TSource>(this IEnumerable<TSource> seq, object value)
    {
        return seq.Where(s => Object.Equals(value, s));
    }
    
    public static IEnumerable<TSource> WhereRegex<TSource>(this IEnumerable<TSource> seq, string regex)
    {
        return seq.Where(s => Regex.IsMatch(s + "", regex));
    }
   
    public static IEnumerable<TSource> Where<TSource>(this IEnumerable<TSource> seq, string property, object value)
    {
        var prop = typeof(TSource).GetProperty(property);
        return seq.Where(s => Object.Equals(value, prop.GetValue(s)));
    }
    
    public static IEnumerable<TSource> WhereRegex<TSource>(this IEnumerable<TSource> seq, string property, string regex)
    {
        var prop = typeof(TSource).GetProperty(property);
        return seq.Where(s => Regex.IsMatch(prop.GetValue(s) + "", regex));
    }
}