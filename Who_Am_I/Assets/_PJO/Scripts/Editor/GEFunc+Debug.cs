using System;
using UnityEditor;
using UnityEngine;

public static partial class GEFunc
{
    public static void DebugNonFind(this SerializedProperty object_, SerializedPropertyType type_)
    {
        Debug.Log($"{object_} not found propertyType to {type_}");
    }

    public static void DebugNonFindComponent(this SerializedProperty object_, Type type_)
    {
        Debug.Log($"{object_} not found {type_}");
    }
}