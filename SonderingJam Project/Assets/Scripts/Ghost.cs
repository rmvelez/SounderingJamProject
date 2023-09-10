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

    private Vector3 startingPos;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;

        player = gameManager.playerController.gameObject;
        target = player.transform;

        startingPos = transform.position;

        task.unCompleteTask();
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        vectorToPlayer = this.gameObject.transform.position - target.position;
        distanceToPlayer = vectorToPlayer.magnitude;

        if(distanceToPlayer < chaseDistance) { 
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed);
        }
    }

    private void OnTriggerStay(UnityEngine.Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject interactBox = other.gameObject;
            if(interactBox.GetComponent<InteractManager>() != null)
            {
            InteractManager interactManager = interactBox.GetComponent<InteractManager>();

                if (interactManager.getAttacking())
                {
                    Debug.Log("ghost is kill");
                }
            } else
            {
                Debug.Log("IUM null");

            }
        }
    }

    public void Kill()
    {
        Debug.Log("ghost is kill");
        gameObject.SetActive(false);
        transform.position = startingPos;
        GameManager.Instance.DespawnGhost(task);

    }

    public void Spawn()
    {
        task.unCompleteTask();
        gameObject.SetActive(true);

    }

}