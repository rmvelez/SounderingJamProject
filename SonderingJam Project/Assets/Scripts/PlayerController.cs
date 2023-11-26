using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using static UnityEditor.Timeline.TimelinePlaybackControls;

enum PlayerDirection { Left, Right, Up , Down, UpLeft, UpRight, DownLeft, DownRight}

public class PlayerController : MonoBehaviour
{
    private GameManager gameManager;

    [SerializeField] private Rigidbody2D rigidBody;
    
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] public Collider2D interractionCollider;
    private GameObject InteractBox;
    [SerializeField] public Collider2D hitboxCollider;
    [SerializeField] private float invincibilityDuration = 1f;
    private float timeSinceHit = 0f;
    [Tooltip("the interact manager for the player")]
    [SerializeField] public InteractManager interactManager;

    [Header("movement")]
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private float playerSpeed;
    [SerializeField] private PlayerDirection playerDirection;
    [SerializeField] private Vector2 moveInput;

    [Header("knockback")]
    [SerializeField] RaycastHit2D boxCast;
    [SerializeField] private float knockBackForce = 2;
    [SerializeField] private float knockBackSpeed = 5;
    [SerializeField] private LayerMask wallLayerMask;
    public bool touchingWall = false;


    [Header("animation")]
    [SerializeField] private Animator playerAnimator;

    public Minigame currentMinigame;

    //Animation Variables
    private int movementAnimationDirection;
    private float prevAngle = 0;
    private int prevAnimDirection;
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
        rigidBody.interpolation = RigidbodyInterpolation2D.Extrapolate;
        
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        gameManager = GameManager.Instance;
        gameManager.playerController = this;

    }

    private void Start()
    {
        gameManager = GameManager.Instance;
        InteractBox = interactManager.gameObject;

        gameManager.onGamePause.AddListener(SwitchActionMapUI);
        gameManager.onGameResume.AddListener(SwitchActionMapPlayer);
        SwitchActionMapPlayer();
    }

    public void OnDestroy()
    {
        gameManager.onGamePause.RemoveListener(SwitchActionMapUI);
        gameManager.onGameResume.RemoveListener(SwitchActionMapPlayer);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move(moveInput);

        if(timeSinceHit <= invincibilityDuration)
        {
            timeSinceHit += Time.deltaTime;
        }
    }

    public void PauseActionPerformed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if(gameManager.paused) //if we're already paused
            {
                gameManager.ResumeGame(); //resume
            } else {
                gameManager.PauseGame();
            }

        }
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
            touchingWall = false;

            Vector3 targetPosition = new Vector3(this.transform.position.x + direction.y, this.transform.position.y - direction.x, 0);
            Vector3 dir = targetPosition - this.transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            //this.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            //InteractBox.transform.RotateAround(transform.position, Vector3.forward, angle);// Quaternion.AngleAxis(angle, Vector3.forward);
            //InteractBox.transform.Rotate(Vector3.forward, angle, Space.Self);
            
            if(angle != prevAngle)
            {
                InteractBox.transform.RotateAround(transform.position, Vector3.forward, -prevAngle );
                InteractBox.transform.RotateAround(transform.position, Vector3.forward, angle);
            }



            if (direction.x < 0)//if we're moving left
            {

                GetComponent<SpriteRenderer>().flipX = true;
                movementAnimationDirection = WALK_SIDE_DIRECTION;

                //flip sprite left
            }
            else if (direction.x > 0) //if we're moving right
            {
                GetComponent<SpriteRenderer>().flipX = false;

                //flip sprite right

                movementAnimationDirection = WALK_SIDE_DIRECTION;
            }
            
            if (direction.y > 0) //override if the player is moving up
            {
                movementAnimationDirection = WALK_UP_DIRECTION;
            }
            else if (direction.y < 0) //if we're moving down
            {
                movementAnimationDirection = WALK_DOWN_DIRECTION;
            }
            prevAngle = angle;

            
            if (playerInput != null)
            {
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
                    movementAnimationDirection = IDLE_SIDE_DIRECTION;
                    break;
                case (WALK_UP_DIRECTION):
                    movementAnimationDirection = IDLE_UP_DIRECTION;
                    break;
                case (WALK_DOWN_DIRECTION):
                    movementAnimationDirection = IDLE_DOWN_DIRECTION;
                    break;
            }


        }

        prevAnimDirection = movementAnimationDirection;

        playerAnimator.SetInteger("Movement", movementAnimationDirection);

    }

            
        
    public void SwitchActionMapPlayer() { SwitchActionMap("Player"); }
    public void SwitchActionMapMinigame() { SwitchActionMap("Minigame"); }
    public void SwitchActionMapUI() { SwitchActionMap("PauseMenu"); }
    public void SwitchActionMap(string mapName)
    {
        Debug.Log("switching action map");
        playerInput.currentActionMap.Disable();
        playerInput.SwitchCurrentActionMap(mapName);

        switch (mapName)
        {
            case "PauseMenu":
                UnityEngine.Cursor.visible = true;
                UnityEngine.Cursor.lockState = CursorLockMode.None;
                break;
            case "Minigame":
                gameManager.bPlayerInMinigame = true;
                UnityEngine.Cursor.visible = true;
                UnityEngine.Cursor.lockState = CursorLockMode.None;
                break;
            default:
            case "Player":
                gameManager.bPlayerInMinigame = false;
                UnityEngine.Cursor.visible = false;
                UnityEngine.Cursor.lockState = CursorLockMode.Locked;
                break;
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("player hit by ghost(?)");
        if (other.CompareTag("Ghost"))
        {
            if(invincibilityDuration <= timeSinceHit)
            {
                playerHit(other.gameObject);
                timeSinceHit = 0;
            }
        }
    }
    //private void OnCollisionEnter2D(Collision2D other)
    //{
    //        //Debug.Log("player hit by ghost(?)");
    //    if (other.gameObject.CompareTag("Ghost")) 
    //    {
    //        playerHit(other.gameObject);
    //    }
    //}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 6)
        {
            //touchingWall = true;
        }
    }
    
    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.gameObject.layer == 6)
        {
            //touchingWall = false;

        }
    }

    private void playerHit(GameObject other)
    {

        Debug.Log("player hit by ghost.");
        //implement knockback? somehow?
        gameManager.IncreaseSress(20);

        Vector2 force = gameObject.transform.position - other.transform.position; 
        //Debug.Log(force.ToString());

        force.Normalize();
        //Debug.Log(force.ToString());

        //figure out what I'm doing wrong here
        //rigidBody.AddForce(force * knockBackForce, ForceMode2D.Impulse);
        RaycastHit2D raycast = Physics2D.Raycast(transform.position, force, (force * knockBackForce).magnitude);
        

        StartCoroutine(Knockback((Vector2)gameObject.transform.position + (force * knockBackForce), other.GetComponent<Ghost>()));
    }

    private IEnumerator Knockback(Vector2 destination, Ghost ghost)
    {

        //RaycastHit2D raycast = Physics2D.Raycast(transform.position, destination.normalized, 2 * knockBackSpeed * Time.deltaTime, 6);
        //Debug.DrawRay(raycast.centroid, raycast.point , Color.red, .1f);

        boxCast =  Physics2D.BoxCast(
            transform.position, hitboxCollider.bounds.size, 0f, 
            destination - (Vector2)gameObject.transform.position,  
            2 *knockBackSpeed*Time.deltaTime, wallLayerMask);

        

        touchingWall = (boxCast.collider != null);



        //Debug.DrawLine(raycast.point, raycast.)
        //Debug.Log(raycast.distance);
        spriteRenderer.color = Color.red; //if we wanted to flash then we'd need a separate coroutine yielding wait.1 second or however long
        //while ((((Vector2) gameObject.transform.position - destination).magnitude > 0.1) && (!touchingWall))
        while ((((Vector2) gameObject.transform.position - destination).magnitude > 0.1) && (!touchingWall))
        {
            boxCast = Physics2D.BoxCast(transform.position, hitboxCollider.bounds.size, 0f,
                destination - (Vector2)gameObject.transform.position, 2 * knockBackSpeed * Time.deltaTime, wallLayerMask);
            touchingWall = (boxCast.collider != null);


            rigidBody.MovePosition(Vector2.MoveTowards(gameObject.transform.position, destination, knockBackSpeed * Time.deltaTime));
            yield return new WaitForFixedUpdate();
        }
        touchingWall = (boxCast.collider != null);
        spriteRenderer.color = Color.white;
        if (touchingWall)
        {
            ghost.moving = false;
        }
        yield return null;
    }


    public void QuitMiniGame(InputAction.CallbackContext context)
    {
        currentMinigame.Lose();
    }

}
