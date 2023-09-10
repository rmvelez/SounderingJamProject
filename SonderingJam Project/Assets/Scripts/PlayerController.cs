using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

enum PlayerDirection { Left, Right, Up , Down, UpLeft, UpRight, DownLeft, DownRight}

public class PlayerController : MonoBehaviour
{
    private GameManager gameManager;

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
    //private const int WALK_LEFT_DIRECTION = 2;
    //private const int WALK_RIGHT_DIRECTION = 2;
    private const int WALK_SIDE_DIRECTION = 2;
    private const int WALK_UP_DIRECTION = 3;
    private const int WALK_DOWN_DIRECTION = 4;
    //private const int IDLE_LEFT_DIRECTION = 0;
    //private const int IDLE_RIGHT_DIRECTION = 0;
    private const int IDLE_SIDE_DIRECTION = 0;
    private const int IDLE_UP_DIRECTION = 1;
    private const int IDLE_DOWN_DIRECTION = 5;



    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        if (playerInput == null)
        {
            Debug.LogError("player controller added a gameObject that doesn't have a PlayerInput on it -- which is definitely a bug");
        }
        movementAnimationDirection = IDLE_DOWN_DIRECTION;
        
    }

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
        gameManager.playerController = this;
        
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
        Debug.Log(" 1");
            rigidBody.velocity = direction * playerSpeed;


        }
        Debug.Log(" 2");

        if (!Mathf.Approximately(direction.x, 0) || !Mathf.Approximately(direction.y, 0))//if we're inputting movement
        {
            Debug.Log(" 3");

            Vector3 targetPosition = new Vector3(this.transform.position.x + direction.y, this.transform.position.y - direction.x, 0);
            Vector3 dir = targetPosition - this.transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            //this.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);



            if (direction.x < 0)//if we're moving left
            {
                Debug.Log(" 3.1");

                GetComponent<SpriteRenderer>().flipX = true;
                movementAnimationDirection = WALK_SIDE_DIRECTION;

                //flip sprite left
            }
            else if (direction.x > 0) //if we're moving right
            {
                Debug.Log(" 3.2");

                GetComponent<SpriteRenderer>().flipX = false;

                //flip sprite right

                movementAnimationDirection = WALK_SIDE_DIRECTION;
            }
            
            if (direction.y > 0) //override if the player is moving up
            {
                Debug.Log(" 3.3");

                movementAnimationDirection = WALK_UP_DIRECTION;
            }
            else if (direction.y < 0) //if we're moving down
            {
                Debug.Log(" 3.4");
                //flip sprite down

                movementAnimationDirection = WALK_DOWN_DIRECTION;
            }

        }
        else //mot moving
        {
            if((movementAnimationDirection != IDLE_DOWN_DIRECTION) && (movementAnimationDirection != IDLE_UP_DIRECTION) && (movementAnimationDirection != IDLE_SIDE_DIRECTION))
            {

            }
            switch (movementAnimationDirection)
            {
                case(IDLE_DOWN_DIRECTION):
                case (IDLE_SIDE_DIRECTION):
                case (IDLE_UP_DIRECTION):
                    break;

                default:
                case (WALK_SIDE_DIRECTION):
                    Debug.Log("ping");
                    movementAnimationDirection = IDLE_SIDE_DIRECTION;
                    break;
                case (WALK_UP_DIRECTION):
                    movementAnimationDirection = IDLE_UP_DIRECTION;
                    break;
                case (WALK_DOWN_DIRECTION):
                    movementAnimationDirection = IDLE_DOWN_DIRECTION;
                    break;




            }



            /*
                Debug.Log(" 4.1");
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
            }*/

        }
        animator.SetInteger("Movement", movementAnimationDirection);
        Debug.Log(movementAnimationDirection);
    }

        
        
    public void SwitchActionMapPlayer() { SwitchActionMap("Player"); }
    public void SwitchActionMapMinigame() { SwitchActionMap("Minigame"); }
    public void SwitchActionMapUI() { SwitchActionMap("UI"); }
    public void SwitchActionMap(string mapName)
    {
        playerInput.currentActionMap.Disable();
        playerInput.SwitchCurrentActionMap(mapName);

        switch (mapName)
        {
            case "UI":
                UnityEngine.Cursor.visible = true;
                UnityEngine.Cursor.lockState = CursorLockMode.None;
                break;
            case "Minigame":
                Debug.Log("set action map to minigame");
                UnityEngine.Cursor.visible = false;
                UnityEngine.Cursor.lockState = CursorLockMode.Locked;
                break;
            default:
                UnityEngine.Cursor.visible = false;
                UnityEngine.Cursor.lockState = CursorLockMode.Locked;
                break;
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
            Debug.Log("player hit by ghost(?)");
        if (other.CompareTag("Ghost"))
        {
            Debug.Log("player hit by ghost.");
        }
    }

}
