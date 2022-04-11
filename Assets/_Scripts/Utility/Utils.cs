using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static List<T> Shuffle<T> (List<T> listToShuffle)
    {
        for (var i = listToShuffle.Count - 1; i > 0; i--)
        {
            var randomIndex = Random.Range(0, i);
            (listToShuffle[i], listToShuffle[randomIndex]) = (listToShuffle[randomIndex], listToShuffle[i]);
        }

        return listToShuffle;
    }
    
    public static Stack<T> ToStack<T>(List<T> listToConvert)
    {
        var stack = new Stack<T>();
        
        foreach (var item in listToConvert)
            stack.Push(item);

        return stack;
    }
}