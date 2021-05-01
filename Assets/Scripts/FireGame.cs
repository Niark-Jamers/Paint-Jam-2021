using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireGame : MonoBehaviour
{
    public MainMachine machine;

    public GameObject fire;
    public GameObject waterDrop;

    Vector2Int waterDropPos = new Vector2Int(0, 3);

    List<Vector2Int> firePoses = new List<Vector2Int>();
    Dictionary<Vector2Int, GameObject> fires = new Dictionary<Vector2Int, GameObject>();

    void OnEnable()
    {
        int fireCount = Random.Range(3, 6);
        firePoses.Clear();
        fires.Clear();

        for (int i = 0; i < fireCount; i++)
        {
            var posInt = FindRandomPos();
            firePoses.Add(posInt);
            var go = GameObject.Instantiate(fire, Vector3.zero, fire.transform.rotation, transform);
            go.transform.localPosition = GirdPosToWorld(posInt);
            fires[posInt] = go;
        }
    }

    Vector2Int FindRandomPos()
    {
        for (int i = 0; i < 200; i++)
        {
            Vector2Int posInt = new Vector2Int(Random.Range(0, 4), Random.Range(0, 4));
            if (posInt == waterDropPos)
                continue;
            if (firePoses.Contains(posInt))
                continue;
            return posInt;
        }
        return new Vector2Int(2, 3);
    }

    Vector3 GirdPosToWorld(Vector2Int pos)
    {
        return new Vector3(pos.x, pos.y, 0) - Vector3.one / 2.0f - Vector3.one;
    }

    float timeoutSeconds = 0.2f;
    float hTimeout = 0;
    float vTimeout = 0;

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        if (Mathf.Abs(h) > 0.1f && Time.time - hTimeout > timeoutSeconds)
        {
            waterDropPos.x += (int)Mathf.Sign(h);
            hTimeout = Time.time;
        }
        if (Mathf.Abs(v) > 0.1f && Time.time - vTimeout > timeoutSeconds)
        {
            waterDropPos.y += (int)Mathf.Sign(v);
            vTimeout = Time.time;
        }

        waterDropPos.x = Mathf.Clamp(waterDropPos.x, 0, 3);
        waterDropPos.y = Mathf.Clamp(waterDropPos.y, 0, 3);
        Debug.Log(waterDropPos);

        waterDrop.transform.localPosition = GirdPosToWorld(waterDropPos);
        if (fires.TryGetValue(waterDropPos, out var go))
        {
            Debug.Log("Destroy: " + go);
            Object.Destroy(go);
            fires.Remove(waterDropPos);
        }
        if (fires.Count == 0)
        {
            machine.FireStop();
            machine.CloseFireGame();
        }
    }
}
