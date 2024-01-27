using UnityEditor;
using UnityEngine;

public static partial class GEFunc
{
    /// <summary>SerializedProperty를 GameObject로 반환</summary>
    /// <param name="object_">GameObject을 가져올 SerializedProperty</param>
    public static GameObject GetPropertyGameObject(this SerializedProperty object_)
    {
        if (object_ != null && object_.propertyType == SerializedPropertyType.ObjectReference)
        {
            GameObject targetGameObject = object_.objectReferenceValue as GameObject;
            return targetGameObject;
        }
        return null;
    }

    /// <summary>SerializedProperty를 Integer로 반환</summary>
    /// <param name="object_">Integer값을 가져올 SerializedProperty</param>
    public static int GetPropertyInteger(this SerializedProperty object_)
    {
        if (object_ != null && object_.propertyType == SerializedPropertyType.Integer)
        {
            int targetObject = object_.intValue;
            return targetObject;
        }
        return default;
    }

    /// <summary>SerializedProperty를 Float으로 반환</summary>
    /// <param name="object_">Float값을 가져올 SerializedProperty</param>
    public static float GetPropertyFloat(this SerializedProperty object_)
    {
        if (object_ != null && object_.propertyType == SerializedPropertyType.Float)
        {
            float targetObject = object_.floatValue;
            return targetObject;
        }
        return default;
    }

    /// <summary>SerializedProperty를 Boolean으로 반환</summary>
    /// <param name="object_">Boolean값을 가져올 SerializedProperty</param>
    public static bool GetPropertyBoolean(this SerializedProperty object_)
    {
        if (object_ != null && object_.propertyType == SerializedPropertyType.Boolean)
        {
            bool targetObject = object_.boolValue;
            return targetObject;
        }
        return default;
    }

    /// <summary>SerializedProperty를 String으로 반환</summary>
    /// <param name="object_">String값을 가져올 SerializedProperty</param>
    public static string GetPropertyString(this SerializedProperty object_)
    {
        if (object_ != null && object_.propertyType == SerializedPropertyType.String)
        {
            string targetObject = object_.stringValue;
            return targetObject;
        }
        return default;
    }
}