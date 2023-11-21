using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GetScore : MonoBehaviour
{
    public TMP_Text score;

    [SerializeField] private ScoreKeeper scoreKeeper;

    // Start is called before the first frame update
    void Start()
    {
        //gameManager = GameManager.Instance;
        scoreKeeper = ScoreKeeper.Instance;

    }

    // Update is called once per frame
    void Update()
    {
        score.text = scoreKeeper.score.ToString();
    }
}
