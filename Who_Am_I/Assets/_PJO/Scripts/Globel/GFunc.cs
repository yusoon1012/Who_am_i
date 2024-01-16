using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class GFunc
{
    public static GameObject GetChildObject(this GameObject _targetObject, string _objectName)
    {
        GameObject searchResult_ = default;
        GameObject searchTarget_ = default;

        for (int i = 0; i < _targetObject.transform.childCount; i++)
        {
            searchTarget_ = _targetObject.transform.GetChild(i).gameObject;
            if (searchTarget_.name.Equals(_objectName))
            {
                searchResult_ = searchTarget_;
                return searchResult_;
            }
            else
            {
                searchResult_ = GetChildObject(searchTarget_, _objectName);

                if (searchResult_ == null || searchResult_ == default) { /* Pass */}
                else { return searchResult_; }
            }
        }

        return searchResult_;
    }

    public static Transform GetChildObject(this Transform _targetObject, string _objectName)
    {
        Transform searchResult_ = default;
        Transform searchTarget_ = default;

        for (int i = 0; i < _targetObject.childCount; i++)
        {
            searchTarget_ = _targetObject.GetChild(i);
            if (searchTarget_.name.Equals(_objectName))
            {
                searchResult_ = searchTarget_;
                return searchResult_;
            }
            else
            {
                searchResult_ = GetChildObject(searchTarget_, _objectName);

                if (searchResult_ == null || searchResult_ == default) { /* Pass */}
                else { return searchResult_; }
            }
        }

        return searchResult_;
    }

    /*
     * GameObject 형식의 오브젝트를 받아
     * 컴포넌트를 이미 가지고 있다면 그대로 반환 없다면 생성 후 반환
     */
    public static T AddComponent<T>(this GameObject _object) where T : Component
    {
        return _object.GetComponent<T>() == true ? _object.GetComponent<T>() : _object.AddComponent<T>();
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
     * 부모 오브젝트가 해당 컴포넌트를 가지고 있다면 부모 오브젝트의 컴포넌트 반환 없다면 null 반환
     */
    public static T SetParentComponent<T>(this GameObject _object) where T : Component
    {
        return _object.GetComponentInParent<T>() == true ? _object.GetComponentInParent<T>() : null;
    }

    /*
     * GameObject 형식의 오브젝트를 받아
     * 해당 오브젝트의 정점 배열을 반환
     */
    public static Vector3[] GetMeshVertiesArray(this GameObject _object)
    {
        MeshFilter meshFilter_ = SetComponent<MeshFilter>(_object);
        if (meshFilter_ == null)
        {
            SubmitNonFindText<MeshFilter>(_object);
            return null;
        }
        Mesh mesh_ = meshFilter_.mesh;

        return mesh_.vertices;
    }

    /*
     * GameObject 형식의 오브젝트를 받아
     * 해당 오브젝트의 자식을 배열로 반환
     */
    public static Vector3[] GetChildArray(this GameObject _object)
    {
        Transform targetObj_ = _object.transform;

        Vector3[] array_ = new Vector3[targetObj_.childCount];

        for (int i = 0; i < targetObj_.childCount; i++)
        {
            array_[i] = targetObj_.GetChild(i).position;
        }

        return array_;
    }

    // 값의 부호에 따라 0, +1, -1을 반환
    public static int SignIndicator(float _num)
    {
        if (_num == 0) { return 0; }
        else if (_num > 0) { return +1; }
        else { return -1; }
    }

    // 랜덤 true, false 반환
    public static bool RandomBool()
    {
        return Random.Range(0, 2) == 1;
    }

    // 랜덤 방향 값 0 ~ 360도
    public static int RandomAngle()
    {
        return Random.Range(0, 360);
    }

    // 랜덤 값 호출 (float)
    public static float RandomValueFloat(float _minValue, float _maxValue)
    {
        return Random.Range(_minValue, _maxValue);
    }

    // 랜덤 값 호출 (int)
    public static int RandomValueInt(int _minValue, int _maxValue)
    {
        return Random.Range(_minValue, _maxValue);
    }

    public static float[] ShuffleArray(float[] _array)
    {
        for (int i = _array.Length - 1; i > 0; i--)
        {
            int randIndex = Random.Range(0, i + 1);

            var temp = _array[i];
            _array[i] = _array[randIndex];
            _array[randIndex] = temp;
        }

        return _array;
    }

    // 자식 갯수를 반환
    public static int GetChildCount(this GameObject _object)
    {
        return _object.transform.childCount;
    }

    public static void SubmitNonFindText<T>(this GameObject _object) where T : Component
    {
        Debug.Log($"{_object.name} not found {typeof(T).Name}");
    }

    public static void SubmitNonFindText(this GameObject _object, System.Type _componentType)
    {
        Debug.Log($"{_object.name} not found {_componentType}");
    }

    public static GameObject GetGameObjectToList(List<GameObject> _list, string _objectName)
    {
        foreach (GameObject list in _list)
        {
            if (list.name == _objectName) { return list; }
        }

        return null;
    }

    public static List<T> GetChildComponentList<T>(this GameObject _object) where T : Component
    {
        if (_object.GetChildCount() < 1) { return null; }

        List<T> list = new List<T>();

        foreach (Transform child in _object.transform)
        {
            T component = child.GetComponent<T>();
            if (component != null)
            {
                list.Add(component);
            }
        }

        return list;
    }
}