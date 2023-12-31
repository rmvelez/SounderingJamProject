using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class ToiletMinigameController : Minigame
{
    public AudioClip toiletPlunge;
    private AudioSource toiletSound;

    [Header("objects")]
    [SerializeField] private SpriteRenderer energyBarSprite;
    [SerializeField] private SpriteRenderer progressBarSprite;
    [SerializeField] private SpriteRenderer toiletSprite;
    [SerializeField] private SpriteRenderer plungerSprite;

    private Vector3 energyBarStartPosition;
    private Vector3 energyBarStartScale;
    private Vector3 progressBarStartPosition;
    private Vector3 progressBarStartScale;
    private float plungerTopHeight = .3f; 

    [Header("progress and energy bar variables")]

    [Tooltip("the ratio of how much progress you'll make given how much energy you use")]
    [SerializeField] private float EnergyToProgressRatio = .5f;

    [SerializeField] private float progressMax = 100;
    [Tooltip("how much the progress depreceates every second")]
    [SerializeField] private float progressStep = 2;
    [Tooltip("the current amount of progress the player has")]
    [SerializeField] private float progressValue = 50;
    [Tooltip("progress bar starts at a random value in this range")]
    [SerializeField] private Vector2 progressStart = new Vector2(25, 75); //for added polish randomize this

    [SerializeField] float energyMax = 100;
    [Tooltip("the current amount of energy the player has")]
    [SerializeField] private float energyValue;

    [Tooltip("b in the formula (bt)^e to set the rate at which the player increases energy")]
    [SerializeField] private float energyIncreaseBase = 1; //b in formula
    [Tooltip("e in the formula (bt)^e to set the rate at which the player increases energy")]
    [SerializeField] private float energyIncreaseExponent = 2; //e in formula
    [Tooltip("t in the formula (bt)^e to set the rate at which the player increases energy")]
    [SerializeField] private float energyIncreaseTimeSinceLastUse = 0; //t in formula                                                           
    //formula is that the energy will increase at a rate of (b*t) ^e, b = base, e = exponent, t = time since last used

    //[SerializeField] float 

    protected override void Start()
    {
        base.Start();

        progressValue = Random.Range(progressStart.x, progressStart.y);
        energyBarStartPosition = energyBarSprite.transform.localPosition;
        energyBarStartScale = energyBarSprite.transform.localScale;

        progressBarStartPosition = progressBarSprite.transform.localPosition;
        progressBarStartScale = progressBarSprite.transform.localScale;

        toiletSound = GetComponent<AudioSource>();
    }

    private void Update()
    {
        float energyPercent = (energyValue / energyMax);
        float energyBarHeight = energyBarStartScale.y * energyPercent;
        energyBarSprite.transform.localPosition = new Vector3(0, energyBarHeight - (energyBarHeight / 2) - energyBarStartScale.y/2 , 0);
        energyBarSprite.transform.localScale = new Vector3(energyBarStartScale.x, energyBarHeight , energyBarStartScale.z);
        
        plungerSprite.transform.localPosition = new Vector3(0, energyPercent * plungerTopHeight, 0);
        

        float progressPercent = (progressValue / progressMax) * progressBarStartScale.y;
        float progressBarHeight = progressBarStartScale.y * progressPercent;
        progressBarSprite.transform.localPosition = new Vector3(0, progressBarHeight - (progressBarHeight/2) - progressBarStartScale.y/2, 0);
        progressBarSprite.transform.localScale = new Vector3(progressBarStartScale.x, progressBarHeight, progressBarStartScale.z);


        //.localScale.y -= energyBarHeight;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(minigameActive)
        {
            energyValue = Mathf.Pow(energyIncreaseBase*energyIncreaseTimeSinceLastUse, energyIncreaseExponent); //base = 1, xp = 2, 
            energyValue = Mathf.Clamp(energyValue, 0, 100);


            progressValue -= progressStep *Time.deltaTime;
            if (progressValue <= 0)
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
            progressValue += energyValue * EnergyToProgressRatio;

            energyValue = 0;
            energyIncreaseTimeSinceLastUse = 0;
            toiletSound.PlayOneShot(toiletPlunge, 1f);

            if(progressValue>=progressMax)
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

    override public void ResetValues()
    {
        energyValue = 0;
        progressValue = Random.Range(progressStart.x, progressStart.y);
        energyIncreaseTimeSinceLastUse = 0;
    }


}
