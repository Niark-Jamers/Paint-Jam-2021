using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KilnMachine : MonoBehaviour
{
    public Slider slider;
    public MainMachine machine;

    void OnEnable()
    {
        slider.gameObject.SetActive(true);
        slider.value = GetRandomValue();
    }

    int GetRandomValue()
    {
        int r = Random.Range(0, 10);

        if (r == 7)
            r = 4;

        return r;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
            slider.value -= 1;
        if (Input.GetKeyDown(KeyCode.D))
            slider.value += 1;

        if (slider.value == 7)
        {
            machine.BrokenStop();
            machine.CloseBrokenGame();
            slider.gameObject.SetActive(false);
        }
    }
}
