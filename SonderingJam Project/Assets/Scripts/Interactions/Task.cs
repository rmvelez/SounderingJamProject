using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task : MonoBehaviour, IInteractable
{
    public bool Completed { get; private set; }

    private GameManager gameManager;

    [SerializeField] private Minigame minigame;

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
        minigame.StartMinigame(this);
    }

    public void CompleteTask()
    {
        Debug.Log("task completed");
        gameManager.DecreaseStress();
        Completed = true;

    }

    public void unCompleteTask()
    {
        Completed = false;
        minigame.ResetValues();
    }
}
