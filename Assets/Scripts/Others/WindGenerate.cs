using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Wind/Wind generate")]
public class WindGenerate : MonoBehaviour
{
    [SerializeField] List<Transform> targets;
    [SerializeField, Range (0, 1000)] float minWindForce;
    [SerializeField, Range (0, 1000)] float maxWindForce;
    [SerializeField, Range(0.01f, 10)] float minWindFrequency;
    [SerializeField, Range(0.01f, 10)] float maxWindFrequency;
    float time;
    float windForceLerp;
    float windForce;

    void Start()
    {
        if (minWindForce > maxWindForce)
        {
            Debug.LogWarning("Минимальная сила ветра изменена, т.к. она не может быть больше максимальной");
            minWindForce = maxWindForce;
        }
        if (minWindFrequency > maxWindFrequency)
        {
            Debug.LogWarning("Минимальная частота была изменена, т.к. она не может быть больше максимальной");
            minWindFrequency = maxWindFrequency;
        }
        time = 1 / Random.Range(minWindFrequency, maxWindFrequency);
        windForceLerp = 6 * Random.Range(minWindForce, maxWindForce);
        windForce = 0;
    }

    void Update()
    {
        time -= Time.deltaTime;
        if (time <= 0f)
        {
            windForceLerp = Random.Range(minWindForce, maxWindForce);
            time = 1 / Random.Range(minWindFrequency, maxWindFrequency);
        }
        windForce = Mathf.Lerp(windForce, windForceLerp, Time.deltaTime);
        foreach (Transform target in targets)
            target.Rotate(0, windForce * Time.deltaTime, 0);
    }
}
