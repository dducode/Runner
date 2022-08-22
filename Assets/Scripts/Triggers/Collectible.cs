using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] AudioClip collectibleClip;
    [SerializeField] BonusType bonusType;
    [SerializeField, Range(1, 10)] int bonusValue;
    public BonusType BonusType { get { return bonusType; } }
    public int BonusValue { get { return bonusValue; } }

    void Update() => transform.Rotate(0, 90 * Time.deltaTime, 0);

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerMovement>())
        {
            GameManager.audioManager.PlaySound(collectibleClip);
            Destroy(gameObject);
        }
    }
}
