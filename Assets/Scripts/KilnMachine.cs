using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KilnMachine : MonoBehaviour
{
    public Slider slider;
    public MainMachine machine;

    public AudioClip movement;

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
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.Q))
        {
            slider.value -= 1;
            AudioManager.instance.PlaySFX(movement, 0.4f);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            slider.value += 1;
            AudioManager.instance.PlaySFX(movement, 0.4f);
        }

        if (slider.value == 7)
        {
            machine.BrokenStop();
            machine.CloseBrokenGame();
            slider.gameObject.SetActive(false);
        }
    }
}
