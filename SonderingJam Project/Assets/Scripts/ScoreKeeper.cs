using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{

    public static ScoreKeeper _instance;
    public static ScoreKeeper Instance { get { return _instance; } }


    public float score;

    // Start is called before the first frame update
    void Start()
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
            _instance.score = Mathf.Max( score, this.score);
            //but first, set the current score to the higher of the two
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
