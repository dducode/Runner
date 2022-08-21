using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    [SerializeField] string beginPointName;
    [SerializeField] string endPointName;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject[] levelsPrefab = new GameObject[3];
    [SerializeField] string musicClip;
    public GameObject player { get; private set; }
    private List<GameObject> spawnedLevels = new List<GameObject>();
    public List<GameObject> levels { get { return spawnedLevels; } }

    private void Start()
    {
        BroadcastMessages.AddListener(Messages.RESTART, DestroyChilds);
        BroadcastMessages.AddListener(Messages.SPAWN_CHUNK, SpawnChunk);
        BroadcastMessages<bool>.AddListener(Messages.PAUSE, IsPause);
        player = Instantiate(playerPrefab);

        GameObject firstChunk = Instantiate(levelsPrefab[Random.Range(0, levelsPrefab.Length)]);
        spawnedLevels.Add(firstChunk);
        SpawnChunk();
    }

    private void OnDestroy()
    {
        BroadcastMessages.RemoveListener(Messages.RESTART, DestroyChilds);
        BroadcastMessages.RemoveListener(Messages.SPAWN_CHUNK, SpawnChunk);
        BroadcastMessages<bool>.RemoveListener(Messages.PAUSE, IsPause);
    }

    int FindPointIndex(GameObject chunk, string pointName)
    {
        for (int i = 0; i < chunk.transform.childCount; i++)
            if (chunk.transform.GetChild(i).name == pointName)
                return i;
        
        return -1;
    }

    public void SpawnChunk()
    {
        GameObject newChunk = Instantiate(levelsPrefab[Random.Range(0, levelsPrefab.Length)]);
        GameObject lastChunk = spawnedLevels[spawnedLevels.Count - 1];
        int begin = FindPointIndex(newChunk, beginPointName);
        int end = FindPointIndex(lastChunk, endPointName);
        newChunk.transform.position = 
            lastChunk.transform.GetChild(end).position - 
            newChunk.transform.GetChild(begin).localPosition;
        spawnedLevels.Add(newChunk);

        if (spawnedLevels.Count > 3)
        {
            Destroy(spawnedLevels[0].gameObject);
            spawnedLevels.RemoveAt(0);
        }
    }

    public void DestroyChilds()
    {
        foreach (GameObject chunk in spawnedLevels)
        {
            if (chunk == spawnedLevels[spawnedLevels.Count - 1])
                break;
            for (int i = 0; i < chunk.transform.childCount; i++)
            {
                if (chunk.transform.GetChild(i).tag is "Dynamic")
                    Destroy(chunk.transform.GetChild(i).gameObject);
            }
        }
    }

    public void IsPause(bool isPause) => Time.timeScale = isPause ? 0f : 1f;

    public void OnApplicationPause(bool _pause) => BroadcastMessages<bool>.SendMessage(Messages.PAUSE, _pause);
}
