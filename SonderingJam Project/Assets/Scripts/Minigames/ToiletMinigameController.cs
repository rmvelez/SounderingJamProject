using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class ToiletMinigameController : Minigame
{


    [Header("objects")]
    [SerializeField] private Sprite toiletSprite;
    [SerializeField] private Sprite plungerSprite;
    [SerializeField] private Sprite energyBarSprite;
    [SerializeField] private Sprite progressBarSprite;


    [Header("progress and energy bar variables")]

    [Tooltip("the ratio of how much progress you'll make given how much energy you use")]
    [SerializeField] private float EnergyToProgressRatio = .5f;

    [SerializeField] private float progressMax = 100;
    [Tooltip("how much the progress depreceates every second")]
    [SerializeField] private float progressStep = 1;
    [Tooltip("the current amount of progress the player has")]
    [SerializeField] private float progressAmount = 50;
    [Tooltip("the default value of the progress bar")]
    [SerializeField] private float progressStart = 50;

    [SerializeField] float energyMax = 100;
    [Tooltip("the current amount of energy the player has")]
    [SerializeField] private float energyAmount;

    [Tooltip("b in the formula (bt)^e to set the rate at which the player increases energy")]
    [SerializeField] private float energyIncreaseBase = 1; //b in formula
    [Tooltip("e in the formula (bt)^e to set the rate at which the player increases energy")]
    [SerializeField] private float energyIncreaseExponent = 2; //e in formula
    [Tooltip("t in the formula (bt)^e to set the rate at which the player increases energy")]
    [SerializeField] private float energyIncreaseTimeSinceLastUse = 0; //t in formula                                                           
    //formula is that the energy will increase at a rate of (b*t) ^e, b = base, e = exponent, t = time since last used

    //[SerializeField] float 


    // Update is called once per frame
    void FixedUpdate()
    {
        if(minigameActive)
        {
            energyAmount = Mathf.Pow(energyIncreaseBase*energyIncreaseTimeSinceLastUse, energyIncreaseExponent); //base = 1, xp = 2, 
            energyAmount = Mathf.Clamp(energyAmount, 0, 100);


            progressAmount += progressStep *Time.deltaTime;
            if (progressAmount >= progressMax)
            {
                Lose();
            }
            //progressAmount = Mathf.Clamp(progressAmount, 0, 100);

            energyIncreaseTimeSinceLastUse += Time.deltaTime;

        }
    }

    public void UseEnergy(InputAction.CallbackContext context)
    {    
        if (context.performed)
        {
            Debug.Log("use energy called");
            progressAmount -= energyAmount * EnergyToProgressRatio;

            energyAmount = 0;
            energyIncreaseTimeSinceLastUse = 0;

            if(progressAmount<0)
            {
                Win();
            }

        }
    }

    override public void StartMinigame(Task task)
    {
        base.StartMinigame(task);
        energyIncreaseTimeSinceLastUse = 0;
    }

    override protected void ResetValues()
    {
        energyAmount = 0;
        progressAmount = progressStart;
        energyIncreaseTimeSinceLastUse = 0;
    }


}
