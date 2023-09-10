using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StainClear : MonoBehaviour
{
    public GameObject kitchenObj;
    private KitchenMinigame kitchenMinigame;

    // Start is called before the first frame update
    void Start()
    {
        kitchenMinigame = kitchenObj.GetComponent<KitchenMinigame>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Sponge")
        {
            this.gameObject.SetActive(false);
            kitchenMinigame.germCount++;
        }
    }
}
