using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;
using PathCreation;
using PathCreation.Examples;

public class BeltBoss : MonoBehaviour
{


    public GameObject tomatePrefab;
    public GameObject holder;
    public GameObject beltHolder;

    public PathCreator pcBelt;
    public PathCreator pcFall;

    public MainMachine machineTube;
    public MainMachine machine1;
    public MainMachine machine2;
    public MainMachine machine3;

    float fallTomatoTimer = 0.8f;
    float fallTrueTimer = 0;

    float tomatoTimer = 1f;
    float trueTimer = 0;

    bool isStop = false;
    bool trueIsStop = false;

    void SpawnTomato(PathCreator pc , bool issbelt)
    {
        GameObject parentTmp;
        if (issbelt)
            parentTmp = beltHolder;
        else    
            parentTmp = holder;
        GameObject tmp = Instantiate(tomatePrefab, pc.gameObject.transform.position, pc.gameObject.transform.rotation, parentTmp.transform);
        tmp.GetComponent<PathFollower>().pathCreator = pc;
    }

    void FallTomato()
    {
        if (fallTrueTimer > fallTomatoTimer)
        {
            SpawnTomato(pcFall, false);
            fallTrueTimer = 0;
        }
        fallTrueTimer += Time.deltaTime;
    }

    void BeltTomato()
    {
        if (trueIsStop == true)
        {
            startTomato();
        }
        isStop = false;
        if (trueTimer > tomatoTimer)
        {
            SpawnTomato(pcBelt, true);
            trueTimer = 0;
        }
        trueTimer += Time.deltaTime;
    }

    void stopTomato()
    {
        Debug.Log("STOP TOMATO");
        foreach (Transform t in beltHolder.transform)
        {
            if (t.GetComponent<BaseTomato>().Canstop == true)
                t.gameObject.GetComponent<PathFollower>().speed = 0;
        }
        trueIsStop = true;
    }

    void startTomato()
    {
        Debug.Log("START TOMATO");
        foreach (Transform t in beltHolder.transform)
        {
            t.gameObject.GetComponent<PathFollower>().speed = 5;
        }
        trueIsStop = false;
    }

    void Update()
    {
        if (machineTube.currentState == MainMachine.State.Working)
            FallTomato();
        if (machine1.currentState == MainMachine.State.Working && machine2.currentState == MainMachine.State.Working && machine3.currentState == MainMachine.State.Working)
            BeltTomato();
        else
            isStop = true;
        if (isStop == true && trueIsStop == false)
            stopTomato();
    }
}
