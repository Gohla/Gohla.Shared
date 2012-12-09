using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public static class IEnumerableExtensions
{
    public static void Do<T>(this IEnumerable<T> enumerable, Action<T> action)
    {
        if(enumerable == null)
            throw new ArgumentNullException("enumerable");

        foreach(T item in enumerable)
            action(item);
    }

    public static void Evaluate<T>(this IEnumerable<T> enumerable)
    {
        if(enumerable == null)
            throw new ArgumentNullException("enumerable");

        foreach(T item in enumerable)
            ;
    }

    public static IEnumerable<R> As<R>(this IEnumerable enumerable)
        where R : class
    {
        foreach(object obj in enumerable)
            yield return obj as R;
    }

    public static IEnumerable<T> AsEnumerable<T>(this T item)
    {
        yield return item;
    }

    public static bool IsEmpty<T>(this IEnumerable<T> enumerable)
    {
        if(enumerable == null)
            throw new ArgumentNullException("enumerable");

        ICollection<T> genericCollection = enumerable as ICollection<T>;
        if(genericCollection != null)
            return genericCollection.Count == 0;

        ICollection nonGenericCollection = enumerable as ICollection;
        if(nonGenericCollection != null)
            return nonGenericCollection.Count == 0;

        return !enumerable.Any();
    }

}
