using System.Collections.Generic;
using UnityEngine;

public class PlayScene : SceneController
{
    [SerializeField] string beginPointName;
    [SerializeField] string endPointName;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject[] ChunksPrefab = new GameObject[3];
    public GameObject player { get; private set; }
    private List<GameObject> spawnedChunks = new List<GameObject>();
    public List<GameObject> chunks { get { return spawnedChunks; } }

    private void Start()
    {
        BroadcastMessages.AddListener(Messages.RESTART, DestroyChilds);
        BroadcastMessages.AddListener(Messages.SPAWN_CHUNK, SpawnChunk);
        BroadcastMessages<bool>.AddListener(Messages.PAUSE, IsPause);
        player = Instantiate(playerPrefab);

        GameObject firstChunk = Instantiate(ChunksPrefab[Random.Range(0, ChunksPrefab.Length)]);
        spawnedChunks.Add(firstChunk);
        SpawnChunk();

        GameManager.audioManager.PlayMusic(Resources.Load("Music/" + playMusic) as AudioClip);
    }

    private void OnDestroy()
    {
        BroadcastMessages.RemoveListener(Messages.RESTART, DestroyChilds);
        BroadcastMessages.RemoveListener(Messages.SPAWN_CHUNK, SpawnChunk);
        BroadcastMessages<bool>.RemoveListener(Messages.PAUSE, IsPause);
    }

    void FixedUpdate()
    {
        if (player.transform.position.z > 1000)
        {
            Vector3 position = player.transform.position;
            CharacterController charController = player.GetComponent<CharacterController>();
            position.z -= 1000;
            charController.enabled = false;
            player.transform.position = position;
            charController.enabled = true;

            foreach (GameObject chunk in spawnedChunks)
            {
                position = chunk.transform.position;
                position.z -= 1000;
                chunk.transform.position = position;
            }
        }
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
        GameObject newChunk = Instantiate(ChunksPrefab[Random.Range(0, ChunksPrefab.Length)]);
        GameObject lastChunk = spawnedChunks[spawnedChunks.Count - 1];
        int begin = FindPointIndex(newChunk, beginPointName);
        int end = FindPointIndex(lastChunk, endPointName);
        newChunk.transform.position = 
            lastChunk.transform.GetChild(end).position - 
            newChunk.transform.GetChild(begin).localPosition;
        spawnedChunks.Add(newChunk);

        if (spawnedChunks.Count > 3)
        {
            Destroy(spawnedChunks[0].gameObject);
            spawnedChunks.RemoveAt(0);
        }
    }

    public void DestroyChilds()
    {
        foreach (GameObject chunk in spawnedChunks)
        {
            if (chunk == spawnedChunks[spawnedChunks.Count - 1])
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
