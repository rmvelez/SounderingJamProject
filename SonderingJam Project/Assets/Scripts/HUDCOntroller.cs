using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HUDCOntroller : MonoBehaviour
{


    [SerializeField] private UIDocument uiDoc;

    private GameManager gameManager;

    private VisualElement root;
    private VerticalProgressBar stressBar;



    private float stressMax ;
    private float stressValue;


    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;

        stressMax = gameManager.GetStressMax();
        stressBar = uiDoc.rootVisualElement.Q<VerticalProgressBar>("StressBar");
    }

    // Update is called once per frame
    void Update()
    {
        stressValue = gameManager.getStressMeter();
        stressBar.value = stressValue;
    }
}
