using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private GameObject player;
    private CharacterController playerChar;

    Vector3 startPos;
    Vector3 move;

    void Start()
    {
        startPos = transform.position;
        move = startPos;
    }

    void LateUpdate()
    {
        if (player != null)
        {
            move.z = player.transform.position.z + startPos.z;
            move.x = Mathf.Lerp(move.x, player.transform.position.x / 2, 0.25f);
            if (playerChar.isGrounded)
                move.y = Mathf.Lerp(move.y, player.transform.position.y + startPos.y, 0.05f);
            transform.position = move;
        }
        else
        {
            player = GameObject.FindWithTag("Player");
            if (player != null)
                playerChar = player.GetComponent<CharacterController>();
        }
    }
}
