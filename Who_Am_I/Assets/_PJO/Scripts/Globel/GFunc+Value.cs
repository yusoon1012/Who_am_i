using UnityEngine;

public static partial class GFunc
{
    public static int SignIndicator(float _num)
    {
        if (_num > 0) { return 1; }
        else if (_num < 0) { return -1; }
        else { return 0; }
    }

    public static bool RandomBool()
    {
        return Random.Range(0, 2) == 1;
    }

    public static float RandomAngle()
    {
        return Random.Range(0f, 360f);
    }

    public static float RandomValueFloat(float _minValue, float _maxValue)
    {
        return Random.Range(_minValue, _maxValue);
    }

    public static int RandomValueInt(int _minValue, int _maxValue)
    {
        return Random.Range(_minValue, _maxValue);
    }
}
