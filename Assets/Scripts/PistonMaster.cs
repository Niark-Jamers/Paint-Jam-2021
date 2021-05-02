using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistonMaster : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.parent.GetComponent<MainMachine>().currentState.ToString() == "Broken")
        {
            foreach (Transform child in transform)
                child.gameObject.SetActive(false);
        }
        else
        {
            foreach (Transform child in transform)
                child.gameObject.SetActive(true);
        }
    }
}
