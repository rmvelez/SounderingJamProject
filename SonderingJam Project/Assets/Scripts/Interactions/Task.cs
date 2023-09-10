using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Task : MonoBehaviour, IInteractable
{
    public bool Completed { get; private set; }

    private GameManager gameManager;

    [SerializeField] private Minigame minigame;
    [SerializeField] public Ghost ghost;

    [SerializeField] private Sprite incompleteSprite;
    [SerializeField] private Sprite completeSprite;

    [SerializeField] private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
        gameManager.Tasks.Add(this);
    }



    public void Interact(InteractManager playerInteractManager, PlayerController playerController)
    {
        minigame.StartMinigame(this);
    }

    public void CompleteTask()
    {
        spriteRenderer.sprite = completeSprite;
        Debug.Log("task completed");
        gameManager.DecreaseStress();
        Completed = true;

    }

    public void unCompleteTask()
    {
        spriteRenderer.sprite = incompleteSprite;
        Completed = false;
        minigame.ResetValues();
    }
}
