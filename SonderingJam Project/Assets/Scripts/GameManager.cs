using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance;
    public static GameManager Instance { get { return _instance; } }
    
    [SerializeField] private float stressMeter = 0;
    public float getStressMeter() { return stressMeter; }

    [SerializeField] protected float numGhosts = 0;

    public List<Task> Tasks = new List<Task>();

    [SerializeField] private ScoreKeeper scoreKeeper;
    private float Timer = 0;
    public float finalTimer = 0;

    [Header("balance variables")]
    [Tooltip("how much stress you gain each second when no ghosts are present")]
    [SerializeField] private float stressMeterScale = 0f;
    [Tooltip("the amount by which we scale the amount of ambient stress collection per ghost that is present - g^m in which g is the number of ghosts and m is the modifier")]
    [SerializeField] private float ghostModifier = 3.0f;
    [Tooltip("how much stress you lose from completing a task")]
    [SerializeField] private float taskCompleteDestressAmount = 30f;

    [SerializeField] private float stressMeterMax;

    [Header("Ghost Variables")]
    [SerializeField] private float ghostSpawnTime = 10;
    [SerializeField] private float ghostSpawnTimeVariance;
    [SerializeField] private float timeSinceLastSpawn;

    public PlayerController playerController;

    private void Awake()
    {
        //SINGLETON PATTERN - ensures that there only ever exists a single gamemanager

        //is this the first time we've created this singleton
        if (_instance == null)
        {
            //we're the first gameManager, so assign ourselves to this instance
            _instance = this;

            // don't keep ourselves between levels
        }
        else
        {
            //if there's another one, then destroy this one
            Destroy(this.gameObject);
        }

    }

    public void SpawnGhost()
    {
        if(Tasks.Count > 0)
        {

            numGhosts++;
            int rand = Random.Range(0, Tasks.Count);
            Ghost ghost = Tasks[rand].ghost;
            ghost.Spawn();
            Tasks.RemoveAt(rand);
        }
    }

    public void DespawnGhost(Task task)
    {
        Tasks.Add(task);
        numGhosts--;
    }

    // Start is called before the first frame update
    void Start()
    {
        stressMeter = 33;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private bool Lost;

    private void FixedUpdate()
    {
        Timer += Time.deltaTime;
        //function is Time.Dt * (stressMeterScale +(numghosts^ghostModifer)
        //consider adding another variable to multiply by numghosts (numghosts * newMod) ^ghostModifier
        IncreaseSress((float)Time.deltaTime * ( (float)stressMeterScale + ((float)Mathf.Pow((float)numGhosts, (float)ghostModifier))));
        //Debug.Log("current stress" +  stressMeter);

        timeSinceLastSpawn += Time.deltaTime;

        if(timeSinceLastSpawn >= ghostSpawnTime)
        {
            Debug.Log("game man is saying to spawna  ghost");
            SpawnGhost();
            timeSinceLastSpawn = 0;
        }

        if ((stressMeter >= stressMeterMax) && !Lost) 
        {
            finalTimer = Timer;
            scoreKeeper.score = Timer;
            SceneManager.LoadScene("GameOverScene");
            Lost = true;
        }
    }


    public void DecreaseStress() { DecreaseStress(taskCompleteDestressAmount); }
    public void DecreaseStress(float amount)
    {
        stressMeter = Mathf.Max(stressMeter - amount, 0);
    }

    public void IncreaseSress(float amount)
    {
        //Debug.Log("increasing stress by: "+  amount / Time.deltaTime);
        stressMeter = Mathf.Min(stressMeter + amount, stressMeterMax);
    }
}
