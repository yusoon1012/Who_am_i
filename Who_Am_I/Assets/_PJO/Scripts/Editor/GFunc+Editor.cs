using System;
using System.Linq;
using System.Transactions;
using UnityEditor;
using UnityEngine;

public static partial class GFuncE
{
    #region SerializedProperty
    public static T SetComponent<T>(this SerializedProperty _object) where T : Component
    {
        GameObject targetObject_ = (GameObject)_object.objectReferenceValue;

        return targetObject_.GetComponent<T>() == true ? targetObject_.GetComponent<T>() : null;
    }

    public static GameObject SetGameObject(this SerializedProperty _object)
    =>(GameObject)_object.objectReferenceValue;

    public static T AddComponent<T>(this SerializedProperty _object) where T : Component
    {
        GameObject targetObject_ = (GameObject)_object.objectReferenceValue;

        return targetObject_.GetComponent<T>() == true ? targetObject_.GetComponent<T>() : targetObject_.AddComponent<T>();
    }

    public static Transform SetTransform(this SerializedProperty _object)
    {
        GameObject targetTransform_ = (GameObject)_object.objectReferenceValue;

        return targetTransform_ != null ? targetTransform_.transform : null;
    }

    public static void SubmitNonFindText(this SerializedProperty _object, Type _componentType)
    {
        Debug.Log($"{_object.name} not found {_componentType}");
    }
    #endregion

    #region GameObject
    public static T SetComponent<T>(this GameObject _object) where T : Component
    {
        return _object.GetComponent<T>() == true ? _object.GetComponent<T>() : null;
    }

    public static T AddComponent<T>(this GameObject _object) where T : Component
    {
        return _object.GetComponent<T>() == true ? _object.GetComponent<T>() : _object.AddComponent<T>();
    }

    public static bool HasComponent(this GameObject _object, Type _type)
    {
        return _object.GetComponent(_type) != null ? true : false;
    }

    public static void SubmitNonFindText(this GameObject _object, Type _componentType)
    {
        Debug.Log($"{_object.name} not found {_componentType}");
    }

    public static GameObject PrimitiveObject(this PrimitiveType _type)
    {
        if (!Enum.IsDefined(typeof(PrimitiveType), _type))
        {
            Debug.Log($"{_type} is not a valid PrimitiveType");

            return null;
        }

        GameObject primitiveObject_ = GameObject.CreatePrimitive(_type);

        return primitiveObject_;
    }

    public static Mesh CopyMesh(this Mesh _mesh)
    {
        if (_mesh == null) { return null; }

        Mesh copyMesh_ = new Mesh();

        copyMesh_.vertices = _mesh.vertices;
        copyMesh_.triangles = _mesh.triangles;
        copyMesh_.normals = _mesh.normals;
        copyMesh_.uv = _mesh.uv;
        if (_mesh.name.StartsWith("Copy") == false)
        {
            copyMesh_.name = "Copy" + _mesh.name;
        }
        else { copyMesh_.name = _mesh.name; }

        return copyMesh_;
    }

    public static bool HasChild(this Transform _transform)
    {
        if (_transform.childCount <= 0)
        {
            Debug.Log($"{_transform.name} has no children");
            return false;
        }

        return true;
    }

    public static Type GetType(string _type)
    {
        string typeName_ = Define.UNITY_ENGINE + Define.DOT + _type;

        Type type_ = AppDomain.CurrentDomain.GetAssemblies()
        .SelectMany(s => s.GetTypes())
        .FirstOrDefault(p => p.FullName == typeName_);

        return type_ != null ? type_ : null;
    }
    #endregion
}
