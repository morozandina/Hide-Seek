using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Extensions
{
    private static readonly System.Random Rnd = new System.Random();
    
    public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> source)
    {
        return source.Select((item, index) => (item, index));
    }
    
    public static T PickRandom<T>(this IList<T> source)
    {
        var randIndex = Rnd.Next(source.Count);
        return source[randIndex];
    }
    
    public static bool HasComponent<T>(this GameObject obj) where T : Component
    {
        return (obj.GetComponent<T>()) != null;
    }
}
