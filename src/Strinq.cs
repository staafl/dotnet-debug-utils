using System;
using System.Collections.Generic;
using System.Linq;

public static class Strinq
{
    public static IEnumerable<object> Select<TSource>(this IEnumerable<TSource> seq, string property)
    {
        var prop = typeof(TSource).GetProperty(property);
        return seq.Select(s => prop.GetValue(s));
    }
   
    public static IEnumerable<TSource> Where<TSource>(this IEnumerable<TSource> seq, string property, object value)
    {
        var prop = typeof(TSource).GetProperty(property);
        return seq.Where(s => Object.Equals(value, prop.GetValue(s)));
    }
}