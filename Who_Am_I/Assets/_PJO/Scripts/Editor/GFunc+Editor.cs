using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static partial class GFuncE
{
    /*
     * SerializedProperty 형식의 오브젝트를 받아
     * <T>컴포넌트를 가지고 있는지 확인하는 메서드
     */
    public static T PropertySetComponent<T>(this SerializedProperty _object) where T : Component
    {
        GameObject targetObj_ = (GameObject)_object.objectReferenceValue;

        return targetObj_.GetComponent<T>() == true ? targetObj_.GetComponent<T>() : null;
    }

    /*
     * SerializedProperty 형식의 오브젝트를 받아
     * 오브젝트의 transform값 반환
     */
    public static Transform PropertySetTransform(this SerializedProperty _object)
    {
        GameObject targetTra_ = (GameObject)_object.objectReferenceValue;

        return targetTra_ != null ? targetTra_.transform : null;
    }

    /*
     * GameObject 형식의 오브젝트를 받아
     * 컴포넌트를 가지고 있다면 그대로 반환 없다면 null 반환
     */
    public static T SetComponent<T>(this GameObject _object) where T : Component
    {
        return _object.GetComponent<T>() == true ? _object.GetComponent<T>() : null;
    }

    /*
     * GameObject 형식의 오브젝트를 받아
     * 컴포넌트를 이미 가지고 있다면 그대로 반환 없다면 생성 후 반환
     */
    public static T AddComponent<T>(this GameObject _object) where T : Component
    {
        return _object.GetComponent<T>() == true ? _object.GetComponent<T>() : _object.AddComponent<T>();
    }
}
