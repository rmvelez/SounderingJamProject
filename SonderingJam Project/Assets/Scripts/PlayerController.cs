using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

enum PlayerDirection { Left, Right, Up , Down, UpLeft, UpRight, DownLeft, DownRight}

public class PlayerController : MonoBehaviour
{

    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] public Collider2D interractionCollider;
    [SerializeField] public Collider2D hitboxCollider;
    [Tooltip("the interact manager for the player")]
    [SerializeField] public InteractManager interactManager;

    [Header("movement")]
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private float playerSpeed;
    [SerializeField] private PlayerDirection playerDirection;
    [SerializeField] private Vector2 moveInput;
    [SerializeField] private Animator animator;

    //Animation Variables
    private int movementAnimationDirection;
    private const int WALK_LEFT_DIRECTION = 2;
    private const int WALK_RIGHT_DIRECTION = 2;
    private const int WALK_UP_DIRECTION = 3;
    private const int WALK_DOWN_DIRECTION = 4;
    private const int IDLE_LEFT_DIRECTION = 0;
    private const int IDLE_RIGHT_DIRECTION = 0;
    private const int IDLE_UP_DIRECTION = 1;
    private const int IDLE_DOWN_DIRECTION = 5;



    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        if (playerInput == null)
        {
            Debug.LogError("player controller added a gameObject that doesn't have a PlayerInput on it -- which is definitely a bug");
        }

        
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move(moveInput);
    }

    public void MoveActionPerformed(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void InteractActionPerformed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            interactManager.Interact();
        }
    }

    private void Move(Vector2 direction)
    {
        if (playerInput != null)
        {
            rigidBody.velocity = direction * playerSpeed;


        }

        if (!Mathf.Approximately(direction.x, 0) || !Mathf.Approximately(direction.y, 0))//if we're inputting movement
        {
            Vector3 targetPosition = new Vector3(this.transform.position.x + direction.y, this.transform.position.y - direction.x, 0);
            Vector3 dir = targetPosition - this.transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            //this.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);



            if (direction.x < 0)//if we're moving left
            {

                GetComponent<SpriteRenderer>().flipX = true;
                movementAnimationDirection = WALK_LEFT_DIRECTION;

                //flip sprite left
            }
            else if (direction.x > 0) //if we're moving right
            {

                GetComponent<SpriteRenderer>().flipX = false;

                //flip sprite right

                movementAnimationDirection = WALK_RIGHT_DIRECTION;
            }
            else if (direction.y > 0) //and the player is moving up
            {
                movementAnimationDirection = WALK_UP_DIRECTION;
            }
            else if (direction.y < 0) //if we're moving down
            {
                //flip sprite down

                movementAnimationDirection = WALK_RIGHT_DIRECTION;
            }

        }
        else //mot moving
        {
            if (direction.x < 0)//if we're moving left
            {
                GetComponent<SpriteRenderer>().flipX = true;
                movementAnimationDirection = IDLE_LEFT_DIRECTION;
            }
            else if (direction.x > 0) //if we're moving right
            {
                //flip sprite right

                GetComponent<SpriteRenderer>().flipX = false;
                movementAnimationDirection = IDLE_RIGHT_DIRECTION;
            }
            else if (direction.y > 0) //and the player is moving up
            {
                movementAnimationDirection = IDLE_UP_DIRECTION;
            }
            else if (direction.y < 0) //if we're moving down
            {
                //flip sprite down

                movementAnimationDirection = IDLE_RIGHT_DIRECTION;
            }

        }

        animator.SetInteger("Movement", movementAnimationDirection);


    }
}
