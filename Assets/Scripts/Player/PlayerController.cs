using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerData))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController))]
[AddComponentMenu("Player/Player controller")]
public class PlayerController : MonoBehaviour
{
    [SerializeField] AudioClip deathSound;
    Behaviour playerMovementBehaviour;
    PlayerMovement playerMovement;
    PlayerData playerData;
    Animator animator;
    CharacterController charController;

    void OnEnable()
    {
        BroadcastMessages.AddListener(MessageType.DEATH, Death);
        BroadcastMessages.AddListener(MessageType.RESTART, Restart);
        BroadcastMessages<bool>.AddListener(MessageType.PAUSE, IsPause);
    }
    void OnDisable()
    {
        BroadcastMessages.RemoveListener(MessageType.DEATH, Death);
        BroadcastMessages.RemoveListener(MessageType.RESTART, Restart);
        BroadcastMessages<bool>.RemoveListener(MessageType.PAUSE, IsPause);
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        playerMovementBehaviour = GetComponent<PlayerMovement>();
        playerData = GetComponent<PlayerData>();
        playerMovement = GetComponent<PlayerMovement>();
        charController = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (playerMovementBehaviour.enabled)
        {
            playerData.SetScore(playerMovement.playerSpeed);
            animator.SetBool("jumping", !charController.isGrounded);
            float animationSpeed = Mathf.InverseLerp(
                playerMovement.minRunSpeed, playerMovement.maxRunSpeed, playerMovement.playerSpeed
                );
            animationSpeed *= 0.5f;
            animator.speed = 1 + animationSpeed;
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if ((charController.collisionFlags & CollisionFlags.Sides) is not 0 && hit.collider.CompareTag("Barriers"))
        {
            BroadcastMessages.SendMessage(MessageType.DEATH);
            Managers.audioManager.PlaySound(deathSound);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SpawnTrigger"))
            BroadcastMessages.SendMessage(MessageType.SPAWN_CHUNK);
    }

    public void Death()
    {
        animator.SetBool("death", true);
        animator.SetBool("jumping", false);
        animator.speed = 1;
        playerMovementBehaviour.enabled = false;
    }
    public void Restart()
    {
        animator.SetBool("death", false);
        StartCoroutine(AwaitPlayer());
    }

    IEnumerator AwaitPlayer()
    {
        yield return null;
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        while (!info.IsName("Base.Run"))
        {
            info = animator.GetCurrentAnimatorStateInfo(0);
            yield return null;
        }
        playerMovementBehaviour.enabled = true;
    }
    
    public void IsPause(bool isPause) => playerMovementBehaviour.enabled = !isPause;
}
