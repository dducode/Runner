using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Prize", menuName = "Prize", order = 51)]
public class Prizes : ScriptableObject
{
    [SerializeField] int moneys;
    [SerializeField] int health;

    public int Moneys => moneys;
    public int Health => health;
}
