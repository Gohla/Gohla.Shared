using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

public static class EnumerableToString
{
    public static IObservable<String> ToString<T>(this IObservable<IEnumerable<T>> source, String separator)
    {
        if(source == null)
            throw new ArgumentException("Parameter source can not be null.");

        if(String.IsNullOrEmpty(separator))
            throw new ArgumentException("Parameter separator can not be null or empty.");

        return source
            .Select(n => n.ToString(separator))
            ;
    }

    public static String ToString<T>(this IEnumerable<T> source, String separator)
    {
        if (source == null)
            throw new ArgumentException("Parameter source can not be null.");

        if (String.IsNullOrEmpty(separator))
            throw new ArgumentException("Parameter separator can not be null or empty.");

        String[] array = source
            .Where(n => n != null)
            .Select(n => n.ToString())
            .ToArray()
            ;

        return String.Join(separator, array);
    }

    public static String ToString(this IEnumerable source, String separator)
    {
        if (source == null)
            throw new ArgumentException("Parameter source can not be null.");

        if(String.IsNullOrEmpty(separator))
            throw new ArgumentException("Parameter separator can not be null or empty.");

        String[] array = source
            .Cast<object>()
            .Where(n => n != null)
            .Select(n => n.ToString())
            .ToArray()
            ;

        return String.Join(separator, array);
    }
}