using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class electicityLever : MonoBehaviour
{
    public MainMachine machine;
    public Image leverImage;
    public RectTransform pos;
    public float targetPos;
    private Vector3 defaultPos;
    public AudioSource goodBoySound;
    public float speed = 1f;
    
    // Start is called before the first frame update
    void Start()
    {
        defaultPos = new Vector3(pos.transform.localPosition.x, pos.transform.localPosition.y, pos.transform.localPosition.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Z))
        {
            if (pos.transform.localPosition.y < targetPos)
            {
                pos.transform.localPosition =  new Vector3 (pos.transform.localPosition.x, pos.transform.localPosition.y + (speed * Time.deltaTime), pos.transform.localPosition.z);
            }
            else
            {
                pos.transform.localPosition = new Vector3(defaultPos.x, defaultPos.y, defaultPos.z);
                AudioManager.instance.PlaySFXPitch3(goodBoySound.clip);
                machine.BrokenStop();
                GameManager.instance.RepairElectricity();
                machine.CloseBrokenGame();
            }
        }
        else
        {
            Debug.Log(pos.transform.localPosition.y);
            Debug.Log(defaultPos.y);
            if (pos.transform.localPosition.y > defaultPos.y)
            {
                Debug.Log(-speed * Time.deltaTime);
                pos.transform.localPosition = new Vector3(pos.transform.localPosition.x, (pos.transform.localPosition.y + (-speed * Time.deltaTime)), pos.transform.localPosition.z);
            }
            else
            {
                pos.transform.localPosition = new Vector3(defaultPos.x, defaultPos.y, defaultPos.z);
            }
        }
    }
}
