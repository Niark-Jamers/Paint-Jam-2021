using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{
    public GameObject branch;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float t = GameManager.instance.GetLevelTimeBetween01();
        float r = Mathf.PI * 2 * t;
        branch.transform.rotation = Quaternion.Euler(0, 0, -Mathf.Rad2Deg * r + 90);
    }
}
