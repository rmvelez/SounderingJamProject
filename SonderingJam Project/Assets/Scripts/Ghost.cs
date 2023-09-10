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


    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;

        player = gameManager.playerController.gameObject;
        target = player.transform;

        task.unCompleteTask();
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

}