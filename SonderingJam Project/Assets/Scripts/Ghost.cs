using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



public class Ghost : MonoBehaviour
{
    GameManager gameManager;

    [SerializeField] private Task task;

    [SerializeField] private float speed;
    private GameObject player;

    public float distanceToPlayer;
    private Transform target;

    private Vector2 vectorToPlayer;

    [SerializeField] private float chaseDistance;

    private SpriteRenderer spriteRenderer;
    private Collider2D collider;

    private Vector3 startingPos;

    [SerializeField] private AudioClip ghostAppear;
    [SerializeField] private AudioClip ghostAttack;
    [SerializeField] private AudioClip ghosthurt;
    [SerializeField] private AudioSource ghostSound;


    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;

        player = gameManager.playerController.gameObject;
        target = player.transform;

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
        if(target != null)
        {
        vectorToPlayer = this.gameObject.transform.position - target.position;

        } else
        {
            Debug.Log("uhuh");
        }
        distanceToPlayer = vectorToPlayer.magnitude;

        if(distanceToPlayer < chaseDistance) { 
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed);
        }
    }


    public void Kill()
    {
        //ghostSound.PlayOneShot(ghosthurt);
        Debug.Log("ghost is kill");
        StartCoroutine(playThenDestroy(ghosthurt));
        transform.position = startingPos;
        Debug.Log("resetting pos");
    }

    private IEnumerator playThenDestroy(AudioClip clip)
    {
        //player.GetComponentInChildren<InteractManager>().UntrackObject(gameObject);
        GameManager.Instance.DespawnGhost(task);
        spriteRenderer.enabled = false;
        collider.enabled = false;
        ghostSound.PlayOneShot(clip);

        while (ghostSound.isPlaying)
        {
            yield return null;
        }

        spriteRenderer.enabled = true;
        collider.enabled = true; 
        gameObject.SetActive(false);


    }

    public void Spawn()
    {
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
        }
    }
}