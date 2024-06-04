using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Extension ¸ðÀ½
/// </summary>
public static class Extension
{
    static System.Random _random = new System.Random();
    public static T GetRandom<T>(this IList<T> list)
    {
        int index = _random.Next(list.Count);
        return list[index];
    }

    public static bool IsValid(this GameObject _gameObject)
    {
        return (_gameObject != null) && _gameObject.activeSelf;
    }
}
