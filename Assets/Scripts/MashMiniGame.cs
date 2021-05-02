using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MashMiniGame : MonoBehaviour
{
    public GameObject MiniUi;

    public GameObject MGBg;
    public GameObject MGSlider;

    public MainMachine machine;
    public RectTransform RTSlider;

    public AudioClip mashSound;

    float currentMash = 0;
    float lastMash;

    float maxMash = 100;
    float mashStep = 15;
    
    float mashDown = 30;
    // Start is called before the first frame update
    private void OnEnable() {
        currentMash = 0;
        MiniUi.SetActive(true);
    }




    // Update is called once per frame
    void Update()
    {
        lastMash = currentMash;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AudioManager.instance.PlaySFX(mashSound, 0.3f);
            currentMash += mashStep;
        }
        currentMash -= mashDown * Time.deltaTime;
        currentMash = currentMash < 0? 0 : currentMash;
        RTSlider.localScale = Vector3.Lerp(new Vector3(lastMash / 100, lastMash/100, 1),new Vector3(currentMash / 100, currentMash/100, 1), Time.deltaTime);
    
        if (currentMash > maxMash)
        {
            machine.BrokenStop();
            MiniUi.SetActive(false);
            machine.CloseBrokenGame();
        }
    }
}
