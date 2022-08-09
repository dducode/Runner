using UnityEngine;

public class BarriersTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerMovement>())
            BroadcastMessages.SendMessage(Messages.DEATH);
    }
}
