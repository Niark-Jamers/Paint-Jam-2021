using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TomatoBeltKiller : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Tomato")
            Destroy(other.gameObject);
    }
}

