[System.Serializable]
public class MinMaxRange
{
    public float minValue;
    public float maxValue;

    public float minLimit;
    public float maxLimit;

    public MinMaxRange(float _minLimit, float _maxLimit)
    {
        minLimit = _minLimit;
        maxLimit = _maxLimit;
    }
}
