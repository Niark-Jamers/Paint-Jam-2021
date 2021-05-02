using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{
    public GameObject branch;

    public AudioClip clip;

    bool start = false;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float t = GameManager.instance.GetLevelTimeBetween01();
        float r = Mathf.PI * 2 * t;
        branch.transform.rotation = Quaternion.Euler(0, 0, -Mathf.Rad2Deg * r);

        if (t > 1.0f - 0.166666667 && !start)
        {
            start = true;

            StartCoroutine(Tick());
        }
    }

    IEnumerator Tick()
    {
        for (int i = 0; i < 10; i++)
        {
            AudioManager.instance.PlaySFX(clip, 0.8f);
            yield return new WaitForSeconds(1f);
        }
    }
}
