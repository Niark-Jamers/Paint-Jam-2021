using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricityIsBroken : MonoBehaviour
{
    public MainMachine machine;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (machine.isBroken && !GameManager.instance.electricityBroken)
        {
            GameManager.instance.BreakElectricity();
        }
    }
}
