using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{ 
    public float jumpSpeed = 15.0f;
    public float fallSpeed = -30.0f;
    public float minRunSpeed = 7.5f;
    public float maxRunSpeed = 25f;
    public float targetX { get; private set; }
    public float gravity = -9.8f;    
    private float movedX;
    private float _vertSpeed;
    private float runSpeed;
    public float playerSpeed { get { return runSpeed; } }
    [SerializeField] AudioClip moveSound;
    [SerializeField] AudioClip jumpSound;

    private bool endTouch;

    private Vector3 startPos;
    private Vector3 nextPos;
    private Vector3 movedVector;
    private Vector3 targetPos;
    private Vector3 move;
    private RaycastHit hit;

    private CharacterController _charController;

    void Start()
    {
        _charController = GetComponent<CharacterController>();
        endTouch = true;
        targetX = 0f;
        runSpeed = minRunSpeed;
    }

    void Update()
    {
#if UNITY_ANDROID
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase is TouchPhase.Began)
                startPos = touch.position;
            else if (touch.phase is TouchPhase.Moved)
            {
                nextPos = touch.position;
                movedVector = nextPos - startPos;
                MotionVector();
            }
            else if (touch.phase is TouchPhase.Ended)
                endTouch = true;
        }
#endif

#if UNITY_STANDALONE
        if (Input.GetMouseButtonDown(0))
            startPos = Input.mousePosition;
        else if (Input.GetMouseButton(0))
        {
            nextPos = Input.mousePosition;
            movedVector = nextPos - startPos;
            MotionVector();
        }
        else if (Input.GetMouseButtonUp(0))
            endTouch = true;
#endif

        targetX = Mathf.Lerp(targetX, movedX, 0.2f);
        targetPos = transform.position;
        targetPos.x = targetX;
        move = targetPos - transform.position;
        move.y = _vertSpeed;
        move.x *= runSpeed;
        move.z = runSpeed;
        move = transform.TransformDirection(move);
        _charController.Move(move * Time.deltaTime);

        runSpeed += Time.deltaTime / 60;
        runSpeed = Mathf.Clamp(runSpeed, minRunSpeed, maxRunSpeed);

        if (_charController.isGrounded)
            _vertSpeed = -0.01f;
        else
            _vertSpeed += gravity * 5 * Time.deltaTime;
    }
    private void MotionVector()
    {
        if (movedVector.magnitude >= 25 && endTouch)
        {
            if (Mathf.Abs(movedVector.x) > Mathf.Abs(movedVector.y))
            {
                movedX += movedVector.x > 0 ? 2f : -2f;
                if (movedX >= -2f && movedX <= 2f)
                    GameManager.audioManager.PlaySound(moveSound);
                movedX = Mathf.Clamp(movedX, -2f, 2f);
                endTouch = false;
            }
            else
            {
                if (movedVector.y > 0 && _charController.isGrounded)
                {
                    _vertSpeed = jumpSpeed;
                    GameManager.audioManager.PlaySound(jumpSound);
                    endTouch = false;
                }
                else if (movedVector.y < 0 && !_charController.isGrounded)
                {
                    _vertSpeed = fallSpeed;
                    GameManager.audioManager.PlaySound(moveSound);
                    endTouch = false;
                }
            }
        }
    }
}
