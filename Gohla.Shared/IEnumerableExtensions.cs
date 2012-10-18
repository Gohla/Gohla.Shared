using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Gohla.Shared
{
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
}
