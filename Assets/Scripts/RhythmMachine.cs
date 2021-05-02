using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class RhythmMachine : MonoBehaviour
{
    public MainMachine machine;

    public GameObject keyA;
    public GameObject keyS;
    public GameObject keyD;

    public GameObject keyParent;

    public GameObject line;
    public Slider pointCounter;
    public AudioSource goodBoySound;
    public AudioSource badBoySound;

    public float keySpeed = 2;

    public float distanceThreshold = 0.1f;

    public float timeoutBetweenNotes = 0.3f;

    public float spawnHeight = 5.5f;

    float timeout;
    int points;

    List<GameObject> keysA = new List<GameObject>();
    List<GameObject> keysD = new List<GameObject>();
    List<GameObject> keysS = new List<GameObject>();

    HashSet<GameObject> keyValidated = new HashSet<GameObject>();

    // Start is called before the first frame update
    void OnEnable()
    {
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        if (Random.value < 0.5f && Time.time - timeout > timeoutBetweenNotes)
        {
            SpawnRandomKey();
            timeout = Time.time;
        }

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            bool ok = false;
            foreach (var key in keysA)
                if (IsNearLine(key))
                    ok = AddPoint(key);
            if (!ok)
            {
                points = 0;
                badBoySound.Play();
            }
        }
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            bool ok = false;
            foreach (var key in keysS)
                if (IsNearLine(key))
                    ok = AddPoint(key);
            if (!ok)
            {
                points = 0;
                badBoySound.Play();
            }
        }
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            bool ok = false;
            foreach (var key in keysD)
                if (IsNearLine(key))
                    ok = AddPoint(key);
            if (!ok)
            {
                points = 0;
                badBoySound.Play();
            }
        }

        bool IsNearLine(GameObject key)
        {
            var distance = Mathf.Abs(key.transform.position.y - line.transform.position.y);

            return distance < distanceThreshold;
        }

        bool AddPoint(GameObject keyObject)
        {
            goodBoySound.Play();
            keyValidated.Add(keyObject);
            points++;
            if (points == 3)
            {
                machine.BrokenStop();
                StartCoroutine(Exite());
            }
            return true;
        }

        foreach (var key in keysA.Concat(keysD).Concat(keysS).ToList())
        {
            key.transform.position += new Vector3(0, -1, 0) * Time.deltaTime * keySpeed;

            if (key.transform.position.y < line.transform.position.y - 1)
            {
                Destroy(key);
                if (keyValidated.Contains(key))
                    keyValidated.Remove(key);
                else
                    points = Mathf.Max(0, points - 1);
                keysA.Remove(key);
                keysS.Remove(key);
                keysD.Remove(key);
            }
        }
        
        // Update point counter:
        pointCounter.value = points;
    }

    void Reset()
    {
        foreach (var key in keysA.Concat(keysD).Concat(keysS))
            Object.Destroy(key);
        points = 0;
        keysA.Clear();
        keysS.Clear();
        keysD.Clear();
    }

    void SpawnRandomKey()
    {
        switch (Random.Range(0, 3))
        {
            default:
            case 0:
                keysA.Add(Instantiate(keyA, new Vector3(-1, spawnHeight, 0)));
                break;
            case 1:
                keysS.Add(Instantiate(keyS, new Vector3(0, spawnHeight, 0)));
                break;
            case 2:
                keysD.Add(Instantiate(keyD, new Vector3(1, spawnHeight, 0)));
                break;
        }

        GameObject Instantiate(GameObject prefab, Vector3 pos)
        {
            var go = GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity, keyParent.transform);
            go.transform.localPosition = pos;

            return go;
        }
    }

    IEnumerator Exite()
    {
        yield return new WaitWhile(() => goodBoySound.isPlaying);
        machine.CloseBrokenGame();
    }
}
