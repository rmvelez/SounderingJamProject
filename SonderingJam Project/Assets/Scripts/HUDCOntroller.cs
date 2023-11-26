using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HUDCOntroller : MonoBehaviour
{
    [SerializeField] public SpriteRenderer barSprite;

    [SerializeField] private Vector3 barStartPosition;

    private float stressMax = 100;
    private float stressValue = 0;

    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        stressValue = gameManager.getStressMeter();
        float progressPercent = (stressValue / stressMax) * 9.25f;

        barSprite.transform.localPosition = new Vector3(0, /*energyBarStartPosition.x +*/ barStartPosition.y + (progressPercent / 2), 0);
        barSprite.transform.localScale = new Vector3(2.25f, progressPercent , 9.27f);
    }
}
