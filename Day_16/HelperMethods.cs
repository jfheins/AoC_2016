﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Day_16
{
    public static class HelperMethods
    {
        /// <summary>
        ///    Verpackt den angegebenen Wert in eine Enumeration mit einem Element.
        /// </summary>
        /// <typeparam name="T">Ein beliebiger Typ.</typeparam>
        /// <param name="item">Der Wert, der verpackt werden soll.</param>
        /// <returns>Eine Enumeration, die genau einen Wert enthält.</returns>
        public static IEnumerable<T> ToEnumerable<T>(this T item)
        {
            yield return item;
        }

        /// <summary>
        ///    Liefert zu einer Enumeration alle Paare zurück. Eine Enumeration mit n Elementen hat genau n-1 Paare.
        ///    Die Quelle wird nur einmal durchlaufen. Für jedes Paar wird ein neues Tupel generiert.
        ///    Item1 ist stets das Element, dass in der Quelle zuerst vorkommt.
        /// </summary>
        /// <param name="source">Die Quelle, die paarweise enumeriert werden soll.</param>
        /// <returns>
        ///    Eine Enumeration mit n-1 überschneidenden Tupeln. Gibt eine leere Enumeration zurück, wenn die Quelle aus
        ///    weniger als zwei Elmenten besteht.
        /// </returns>
        public static IEnumerable<Tuple<T, T>> PairwiseWithOverlap<T>(this IEnumerable<T> source)
        {
            using (var it = source.GetEnumerator())
            {
                if (!it.MoveNext())
                    yield break;

                var previous = it.Current;

                while (it.MoveNext())
                    yield return Tuple.Create(previous, previous = it.Current);
            }
        }

        public static IEnumerable<Tuple<T, T>> Pairwise<T>(this IEnumerable<T> source)
        {
            var isPair = false;
            T tempItem = default(T);
            foreach (var item in source)
            {
                if (isPair)
                {
                    yield return Tuple.Create(tempItem, item);
                    isPair = false;
                }
                else
                {
                    tempItem = item;
                    isPair = true;
                }
            }
        }
    }
}