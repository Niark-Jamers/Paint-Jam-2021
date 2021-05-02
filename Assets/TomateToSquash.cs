using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TomateToSquash : MonoBehaviour
{
    public Sprite sp;

     private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Tomato")
        {
            other.GetComponent<BaseTomato>().sp.sprite = sp;
        }
    }
}
