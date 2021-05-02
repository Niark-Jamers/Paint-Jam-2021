using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MonteDescend : MonoBehaviour
{
    
    public GameObject spriteHolder;

    //char up down movement
    float maxY = -10;
    float minY = -13;
    public float speed = 2;
    float realSpeed;
    public float maxTimer = 1.5f;
    public float minTimer = 0.5f;
    float timer = 0;
    bool goingUp = true;


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
        if (spriteHolder.transform.position.y < minY)
        {
            goingUp = true;
        }
        if (spriteHolder.transform.position.y > maxY)
        {
            goingUp = false;
        }
        realSpeed = (goingUp == true) ? speed : -speed;
        timer -= Time.deltaTime;

            transform.position = new Vector3(transform.position.x, transform.position.y + realSpeed * Time.deltaTime, transform.position.z);

    }


    // Update is called once per frame
    void Update()
    {
        MoveUpDown();
    }
}
