using UnityEngine;
using System.Collections.Generic;

public static partial class GFunc
{
    public static bool IsValid(this object object_)
    {
        bool isValid = object_ == null || object_ == default;
        return !isValid;
    }

    public static bool IsValid(this object[] array_)
    {
        bool isValid = array_ == null || array_.Length < 1;
        return !isValid;
    }

    public static bool IsValid<T>(this List<T> list_)
    {
        bool isValid = list_ == null || list_.Count < 1;
        return !isValid;
    }

    public static bool IsValid<T>(this HashSet<T> hash_)
    {
        bool isValid = hash_ == null || hash_.Count < 1;
        return !isValid;
    }

    public static bool IsValid<K, V>(this Dictionary<K, V> dictionary_)
    {
        bool isValid = dictionary_ == null || dictionary_.Count < 1;
        return !isValid;
    }

    public static bool IsValid<T>(this T component_) where T : Component
    {
        Component convert = component_ as Component;
        bool isValid = convert == null || convert == default;
        return !isValid;
    }
}