using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Task : MonoBehaviour, IInteractable
{
    public bool Completed;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void Interact(InteractManager playerInteractManager, PlayerController playerController)
    {
        Debug.Log("task completed");
        Completed = true;
    }
}
