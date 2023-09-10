using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class KitchenMinigame : Minigame
{
    public GameObject wetSponge;

    public GameObject[] dirtyPlates;

    public GameObject[] foodStains;

    public float germCount;

    public float countDown;
    
    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        countDown -= Time.deltaTime;

        // moves the sponge with the mouse
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = Camera.main.transform.position.z + Camera.main.nearClipPlane;
        wetSponge.transform.position = mousePosition;

        if(germCount >= 3)
        {
            dirtyPlates[0].SetActive(false);
            dirtyPlates[1].SetActive(true);
        }
        if(germCount >= 8)
        {
            dirtyPlates[1].SetActive(false);
            dirtyPlates[2].SetActive(true);
        }
        if(germCount >= 16 && countDown >= 0)
        {
            dirtyPlates[2].SetActive(false);
            Win();
        }

        if(countDown <= 0)
        {
            Lose();
        }
    }

    public override void StartMinigame(Task task)
    {
        base.StartMinigame(task);
    }

    override protected void ResetValues()
    {
        foreach(GameObject stain in foodStains)
        {
            stain.SetActive(true);
        }
        dirtyPlates[0].SetActive(true);
        germCount = 0f;
        countDown = 10f;
    }
}
