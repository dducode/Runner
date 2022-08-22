using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarSpawn : MonoBehaviour
{
    [SerializeField] Star bronzeStar;
    [SerializeField] Star silverStar;
    [SerializeField] Star goldStar;

    void Start()
    {
        int chance = Random.Range(0, 100);
        GameObject star;
        if (chance < goldStar.chance)
            star = Instantiate(goldStar.prefab);
        else if (chance < silverStar.chance)
            star = Instantiate(silverStar.prefab);
        else if (chance < bronzeStar.chance)
            star = Instantiate(bronzeStar.prefab);
        else
            star = null;
            
        if (star is not null)
        {
            star.transform.position = transform.position;
            star.transform.SetParent(transform.parent);
        }
    }
}

[System.Serializable]
public struct Star
{
    public GameObject prefab;
    [Range(1, 100)] public int chance;
}
