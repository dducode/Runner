using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerData))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CharacterController))]
[AddComponentMenu("Player/Player controller")]
public class PlayerController : MonoBehaviour
{
    Behaviour playerMovementBehaviour;
    PlayerMovement playerMovement;
    PlayerData playerData;
    Animator animator;
    CharacterController charController;

    void OnEnable()
    {
        BroadcastMessages.AddListener(Messages.DEATH, Death);
        BroadcastMessages.AddListener(Messages.RESTART, Restart);
        BroadcastMessages<bool>.AddListener(Messages.PAUSE, IsPause);
    }
    void OnDisable()
    {
        BroadcastMessages.RemoveListener(Messages.DEATH, Death);
        BroadcastMessages.RemoveListener(Messages.RESTART, Restart);
        BroadcastMessages<bool>.RemoveListener(Messages.PAUSE, IsPause);
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
            animator.speed = 1 + animationSpeed;
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (charController.collisionFlags is CollisionFlags.Sides && hit.transform.tag is "Barriers")
            BroadcastMessages.SendMessage(Messages.DEATH);
    }

    public void Death()
    {
        animator.SetBool("death", true);
        animator.SetBool("jumping", false);
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
