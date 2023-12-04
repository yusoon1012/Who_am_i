using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static partial class GFunc
{
    /*
     * SerializedProperty 형식의 오브젝트를 받아
     * <T>컴포넌트를 가지고 있는지 확인하는 메서드
     */
    public static T PropertyGetComponent<T>(SerializedProperty _object) where T : Component
    {
        GameObject targetObj_ = (GameObject)_object.objectReferenceValue;

        return targetObj_.GetComponent<T>() == true ? targetObj_.GetComponent<T>() : null;
    }

    /*
     * SerializedProperty 형식의 오브젝트를 받아
     * 오브젝트의 transform값 반환
     */
    public static Transform PropertyGetTransform(SerializedProperty _object)
    {
        GameObject targetTra_ = (GameObject)_object.objectReferenceValue;

        return targetTra_ != null ? targetTra_.transform : null;
    }

    /*
     * GameObject 형식의 오브젝트를 받아
     * <T>컴포넌트가 true: 추가하지 않고 반환
     * <T>컴포넌트가 false: <T>컴포넌트를 추가하여 반환
     */
    public static T SetComponent<T>(GameObject _object) where T : Component
    {
        // 오브젝트가 null인지 체크
        if (_object == null)
        {
            Debug.Log("오브젝트를 가져오지 못했습니다.");

            return null;
        }

        return _object.GetComponent<T>() == true ? _object.GetComponent<T>() : _object.AddComponent<T>();
    }

    public static T GetComponent<T>(GameObject _object) where T : Component
    {
        GameObject targetObj_ = _object;

        return targetObj_.GetComponent<T>() == true ? targetObj_.GetComponent<T>() : null;
    }

    public static Vector3[] GetChildArray(GameObject _object)
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
    public static int GetChildCount(GameObject _object)
    {
        return _object.transform.childCount;
    }
}