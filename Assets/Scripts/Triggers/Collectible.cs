using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] AudioClip collectibleClip;
    [SerializeField] BonusType bonusType;
    [SerializeField, Range(1, 10)] int bonusValue;

    void Update() => transform.Rotate(0, 90 * Time.deltaTime, 0);

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerData>())
        {
            BroadcastMessages<BonusType, int>.SendMessage(Messages.COLLECT, bonusType, bonusValue);
            GameManager.audioManager.PlaySound(collectibleClip);
            Destroy(gameObject);
        }
    }
}
