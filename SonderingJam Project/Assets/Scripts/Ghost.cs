using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Ghost : MonoBehaviour
{
    [SerializeField] private Task task;


    // Start is called before the first frame update
    void Start()
    {
        task.unCompleteTask();
    }

    // Update is called once per frame
    void Update()
    {

    }
}