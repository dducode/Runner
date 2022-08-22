using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private GameObject player;

    Vector3 startPos;
    Vector3 move;

    void Start()
    {
        startPos = transform.position;
        move = startPos;
    }

    void LateUpdate()
    {
        if (player is not null)
        {
            move.z = player.transform.position.z + startPos.z;
            move.x = Mathf.Lerp(move.x, player.transform.position.x / 2, Time.deltaTime * 15f);
            move.y = Mathf.Lerp(move.y, (player.transform.position.y / 2) + startPos.y, Time.deltaTime * 15f);
            transform.position = move;
        }
        else
            player = GameObject.FindWithTag("Player");
    }
}
