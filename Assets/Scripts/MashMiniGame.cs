using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MashMiniGame : MonoBehaviour
{

    public GameObject MGBg;
    public GameObject MGSlider;

    public RectTransform RTSlider;

    float currentMash = 0;
    float lastMash;

    float maxMash = 100;
    float mashStep = 10;
    
    float mashDown = 40;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void MashUp()
    {

    }

    // Update is called once per frame
    void Update()
    {
        lastMash = currentMash;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            currentMash += mashStep;
        }
        currentMash -= mashStep * Time.deltaTime;
        currentMash = currentMash < 0? 0 : currentMash;
        RTSlider.localScale = Vector3.Lerp(new Vector3(lastMash / 100, lastMash/100, 1),new Vector3(currentMash / 100, currentMash/100, 1), Time.deltaTime);
    }
}
