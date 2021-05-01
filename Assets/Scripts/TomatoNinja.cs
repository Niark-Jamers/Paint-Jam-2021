using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TomatoNinja : MonoBehaviour
{
    public GameObject target;
    private Vector3 startPos;
    private float lenght;
    private float startTime;

    public float speed = 5;
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        startPos = transform.position;
        lenght = Vector3.Distance(startPos, target.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        float distDone = (Time.time - startTime) * speed;
        float distDonePercent = distDone/lenght;
        transform.position = Vector3.Lerp(transform.position, target.transform.position, distDonePercent);
    }
}
