using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using PathCreation.Examples;

public class TomatoNinja : MonoBehaviour
{
    public GameObject target;
    public GameObject tomatoBin;
    public Animator ninjanimator;

    public GameObject smokeBomb;

    bool death = false;
    bool smoked = false;
    public ParticleSystem smoke;
    public float damage = 40;

    public float rotationSpeed = 180;
    private Vector3 startPos;
    private float lenght;
    private float startTime;

    public float speed = 5;

    bool targetFound = false;

    public GameObject pathHolder;
    public GameObject pathPrefab;

    public PathFollower follow;
    public PathCreator pc;
    GameObject path;
    float flatY = 2;
    float minY = 2;
    float maxY = 5;

    Vector3[] waypoints = new Vector3[3];

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        startPos = transform.position;
        lenght = Vector3.Distance(startPos, target.transform.position);
        path =  Instantiate(pathPrefab, Vector3.zero, Quaternion.Euler(Vector3.zero), pathHolder.transform);
        pc = path.GetComponent<PathCreator>();
    }

    void MoveNinja()
    {
        float distDone = (Time.time - startTime) * speed;
        float distDonePercent = distDone/lenght;
        transform.position = Vector3.Lerp(transform.position, target.transform.position, distDonePercent);
    }

    void NinjaSlice()
    {
        if (death == true)
            return;
        ninjanimator.SetTrigger("ninjaSlice");
    }

    void NinjaFuite()
    {
        if (death == true)
            return;
        smoked = true;
        ninjanimator.SetTrigger("ninjaFuite");
        smokeBomb.SetActive(true);
    }

    public void NinjaSliceOver()
    {
        target.GetComponent<MainMachine>().brokenBar += damage;
        NinjaFuite();
    }


    public void NinjaFuiteOver()
    {
        smoke.Play();
        foreach (Transform t in this.transform)
            Destroy(t.gameObject, 1.5f);
        Destroy(this.gameObject, 5f);
    }

    void NinjaSpin()
    {
        transform.RotateAround(transform.position, transform.forward, Time.deltaTime * rotationSpeed);
    }


    void CreatePathToBin()
    {
        waypoints[0] = transform.position;
        waypoints[1] = new Vector3(tomatoBin.transform.position.x + ((transform.position.x - tomatoBin.transform.position.x) / 2), tomatoBin.transform.position.y + flatY + Random.Range(minY, maxY), 0);
        waypoints[2] = tomatoBin.transform.position;
        BezierPath bezierPath = new BezierPath (waypoints, false, PathSpace.xy);
        pc.bezierPath = bezierPath;
    }

    private void OnDestroy() {
        Destroy(path);
    }

    void GoToNinjaBin()
    {
        follow.pathCreator = pc;
    }

    private void Update() {
        if (death == true && ninjanimator)
        {
            ninjanimator.SetTrigger("ninjaDeath");
            NinjaSpin();
            GoToNinjaBin();
        }
    }



    private void LateUpdate() {
        if (death == true)
            return;
        if (targetFound == false)
            MoveNinja();
        if (targetFound == true && ninjanimator)
        {
            NinjaSlice();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("NINJA TOUCHE " + other.gameObject.name);
        if (other.gameObject.name == target.name)
        {
            targetFound = true;
        }

        if (other.gameObject.tag == "Player")
        {
            if (smoked == true)
                return;
            death = true;
            CreatePathToBin();
        }
    }


}
