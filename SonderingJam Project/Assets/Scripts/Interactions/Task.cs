using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task : MonoBehaviour, IInteractable
{
    public bool Completed;

    private GameManager gameManager;


    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Interact(InteractManager playerInteractManager, PlayerController playerController)
    {
        if(!Completed) { CompleteTask(); }
    }

    protected void CompleteTask()
    {
        Debug.Log("task completed");
        gameManager.DecreaseStress(20);
        Completed = true;

    }
}
