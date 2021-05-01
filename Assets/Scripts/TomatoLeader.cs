using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TomatoLeader : MonoBehaviour
{
    public GameObject[] spawners = new GameObject[5];
    public GameObject[] targets = new GameObject[5];
    public GameObject tomatoPrefab;
    // Start is called before the first frame update

    public float tomatoTimer = 10f;
    float trueTimer = 0;

    GameObject GetTarget()
    {
        int i = Random.Range(0, targets.Length);
        // check if target is down;
        return (targets[i]);
    }

    GameObject GetSpawn()
    {
        int i = Random.Range(0, spawners.Length);
        // check if target is down;
        return (spawners[i]);
    }

    void SpawnTomato()
    {
        GameObject newTomato;
        TomatoNinja newScript;

        newTomato = Instantiate(tomatoPrefab, GetSpawn().transform);
        newScript = newTomato.GetComponent<TomatoNinja>();
        newScript.target = GetTarget();
    }

    void Start()
    {
        
    }

    void Update()
    {
        trueTimer += Time.deltaTime;
        if (trueTimer > tomatoTimer)
        {
            SpawnTomato();
            trueTimer = 0;
        }
    }
}
