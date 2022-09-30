///<summary>
///Кастомное свойство слайдера с возможностью установить в инспекторе минимальное и максимальное значение.
///Используется обычно для установки значений генератора псевдослучайных чисел
///</summary>
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
