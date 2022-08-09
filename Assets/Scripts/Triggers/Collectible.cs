using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] AudioClip collectibleClip;
    void Update()
    {
        transform.Rotate(0, 90 * Time.deltaTime, 0);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerMovement>())
        {
            BroadcastMessages<string>.SendMessage(Messages.COLLECT, tag);
            GameManager.audioManager.PlaySound(collectibleClip);
            Destroy(gameObject);
        }
    }
}
