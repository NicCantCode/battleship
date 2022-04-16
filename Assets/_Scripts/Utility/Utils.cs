using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    private static Dictionary<int, string> battleshipCharacterDictionary = new Dictionary<int, string> {
        {0, "A"},
        {1, "B"},
        {2, "C"},
        {3, "D"},
        {4, "E"},
        {5, "F"},
        {6, "G"},
        {7, "H"},
        {8, "I"},
        {9, "J"}
};
    
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

    public static string GridPositionToBattleshipPositionAsString(Vector2 gridPosition)
    {
        return $"{battleshipCharacterDictionary[(int) gridPosition.x]}{10 - gridPosition.y}";
    }
}