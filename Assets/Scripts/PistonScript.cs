using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PistonScript : MonoBehaviour
{
    
    float maxY = 2.0f;
    float minY = -0.4f;
    public float speed = 2;
    float realSpeed;
    public float maxTimer = 1.0f;
    public float minTimer = 0.5f;
    float timer = 0;
    bool goingUp = false;


    void Start()
    {
        goingUp = (Random.value > 0.5f)? true : false;
    }

    void MoveUpDown()
    {
        if (timer <= 0)
        {
            timer = Random.Range(minTimer, maxTimer);
            goingUp = !goingUp;
        }
        if (transform.position.y < transform.parent.transform.position.y + minY)
        {
            goingUp = true;
            timer = maxTimer + 0.2f;
        }
        if (transform.transform.position.y > transform.parent.transform.position.y + maxY)
        {
            goingUp = false;
            timer = maxTimer + 0.2f;
        }
        realSpeed = (goingUp == true) ? speed : -speed;
        timer -= Time.deltaTime;
        transform.Translate(Vector3.up * realSpeed * Time.deltaTime);        
    }


    // Update is called once per frame
    void Update()
    {
        MoveUpDown();

    }
}
