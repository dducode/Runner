using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Wind/Wind generate")]
public class WindGenerate : MonoBehaviour
{
    [SerializeField] List<Transform> targets;
    [SerializeField] MinMaxRange windForce = new MinMaxRange(0, 1000);
    [SerializeField] MinMaxRange windFrequency = new MinMaxRange(0.01f, 10);
    float time;
    float windForceLerp;
    float _windForce;

    void Start()
    {
        time = 1 / Random.Range(windFrequency.minValue, windFrequency.maxValue);
        windForceLerp = 6 * Random.Range(windForce.minValue, windForce.maxValue);
        _windForce = 0;
    }

    void Update()
    {
        time -= Time.deltaTime;
        if (time <= 0f)
        {
            windForceLerp = Random.Range(windForce.minValue, windForce.maxValue);
            time = 1 / Random.Range(windFrequency.minValue, windFrequency.maxValue);
        }
        _windForce = Mathf.Lerp(_windForce, windForceLerp, Time.smoothDeltaTime);
        foreach (Transform target in targets)
            target.Rotate(0, _windForce * Time.smoothDeltaTime, 0);
    }
}
