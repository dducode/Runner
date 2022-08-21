using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSpawn : MonoBehaviour
{
    [SerializeField] GameObject simpleBox;
    [SerializeField] GameObject silverBox;
    [SerializeField] GameObject goldenBox;

    void Start()
    {
        int chance = Random.Range(0, 100);
        GameObject box;
        switch (chance)
        {
            case >= 98:
                box = Instantiate(goldenBox);
                break;
            case >= 90:
                box = Instantiate(silverBox);
                break;
            case >= 50:
                box = Instantiate(simpleBox);
                break;
            default:
                box = null;
                break;
        }
        if (box is not null)
        {
            box.transform.position = transform.position;
            box.transform.SetParent(transform.parent);
        }
    }
}
