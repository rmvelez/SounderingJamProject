using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minigame : MonoBehaviour
{
    GameManager gameManager;

    protected bool minigameActive = false;
    protected Task parentTask;

    [SerializeField] protected GameObject self;
    
    // Start is called before the first frame update
    virtual protected void Start()
    {
        self = gameObject;

        gameManager = GameManager.Instance;

        self.SetActive(false);
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    virtual protected void Win()
    {
        gameManager.playerController.SwitchActionMapPlayer();
        parentTask.CompleteTask();
        self.SetActive(false);
        ResetValues();
    }

    virtual public void StartMinigame(Task task)
    {
        self.SetActive(true);
        gameManager.playerController.SwitchActionMapMinigame();
        Debug.Log("minigame started");
        parentTask = task; 
        minigameActive = true;
    }

    virtual protected void Lose()
    {
        Debug.Log("minigame lost");
        gameManager.playerController.SwitchActionMapPlayer();
        self.SetActive(false);
        ResetValues();

    }

    [Tooltip("does NOT reset completed")]
    virtual protected void ResetValues()
    {

    }
}
