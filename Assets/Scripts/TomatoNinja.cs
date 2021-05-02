using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TomatoNinja : MonoBehaviour
{
    public GameObject target;
    public GameObject tomatoBin;
    public Animator ninjanimator;

    bool death = false;
    public ParticleSystem smoke;
    public float damage = 40;

    public float rotationSpeed = 180;
    private Vector3 startPos;
    private float lenght;
    private float startTime;

    public float speed = 5;

    bool targetFound = false;
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        startPos = transform.position;
        lenght = Vector3.Distance(startPos, target.transform.position);
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
        ninjanimator.SetTrigger("ninjaFuite");
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

    void GoToNinjaBin()
    {

    }

    private void Update() {
        if (death == true)
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
        Debug.Log("NINJA TOUCHE " + other.gameObject.name);
        if (other.gameObject.name == target.name)
        {
            targetFound = true;
        }

        if (other.gameObject.tag == "Player")
        {
            death = true;
        }
    }


}
