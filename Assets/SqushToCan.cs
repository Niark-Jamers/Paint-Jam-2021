using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using PathCreation.Examples;

public class SqushToCan : MonoBehaviour
{
    public Sprite[] sp = new Sprite[5];

     private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Tomato")
        {
            BaseTomato tmp = other.gameObject.GetComponent<BaseTomato>();
            tmp.Canstop = false;
            tmp.sp.sprite = sp[Random.Range(0, sp.Length)];
            other.GetComponent<PathFollower>().rotFollow = true;
            tmp.tr.rotation = Quaternion.Euler(0, 90, 180);
        }
    }
}
