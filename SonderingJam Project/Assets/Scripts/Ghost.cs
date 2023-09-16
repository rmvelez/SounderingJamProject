using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



public class Ghost : MonoBehaviour
{
    GameManager gameManager;

    [SerializeField] private Task task;

    [SerializeField] private float speed;
    private PlayerController player;

    public float distanceToPlayer;
    private Transform target;

    private Vector2 vectorToPlayer;

    [SerializeField] private float chaseDistance;
    [SerializeField] private float reactivationDistance;

    private SpriteRenderer spriteRenderer;
    private Collider2D collider;

    private Vector3 startingPos;

    [SerializeField] private AudioClip ghostAppear;
    [SerializeField] private AudioClip ghostAttack;
    [SerializeField] private AudioClip ghosthurt;
    [SerializeField] private AudioSource ghostSound;

    public bool moving = true;


    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;

        player = gameManager.playerController;
        target = player.gameObject.transform;

        startingPos = transform.position;

        task.unCompleteTask();
        gameObject.SetActive(false);

        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        collider = gameObject.GetComponent<Collider2D>();

        //ghostSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (target != null)
        {
            vectorToPlayer = this.gameObject.transform.position - target.position;

        }
        else
        {
            Debug.Log("uhuh");
        }
        distanceToPlayer = vectorToPlayer.magnitude;
        if (moving)
        {


            if(distanceToPlayer < chaseDistance) { 
                transform.position = Vector2.MoveTowards(transform.position, target.position, speed);
            }
        } else if(distanceToPlayer > reactivationDistance && !player.touchingWall)
        {
            moving = true;
        }
        moving = !player.touchingWall;
    }


    public void Kill()
    {
        moving = false;
        //ghostSound.PlayOneShot(ghosthurt);
        Debug.Log("ghost is kill");
        StartCoroutine(playThenDestroy(ghosthurt));
        Debug.Log("resetting pos");
    }

    private IEnumerator playThenDestroy(AudioClip clip)
    {
        spriteRenderer.enabled = false;
        collider.enabled = false;
        ghostSound.PlayOneShot(clip);

        while (ghostSound.isPlaying)
        {
            yield return null;
        }

        spriteRenderer.enabled = true;
        transform.position = startingPos;
        collider.enabled = true;
        gameObject.SetActive(false);
        //player.GetComponentInChildren<InteractManager>().UntrackObject(gameObject);
        GameManager.Instance.DespawnGhost(task);


    }

    public void Spawn()
    {
        moving = true;
        if (task.Completed)
        {

            task.unCompleteTask();
        }
        gameObject.SetActive(true);
        ghostSound.PlayOneShot(ghostAppear);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ghostSound.PlayOneShot(ghostAttack);
            moving = false;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        //moving = true;
    }
}