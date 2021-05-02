using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScreen : MonoBehaviour
{
    void Update()
    {
        if (Input.anyKeyDown)
            GameManager.instance.LoadNext();
    }
}
