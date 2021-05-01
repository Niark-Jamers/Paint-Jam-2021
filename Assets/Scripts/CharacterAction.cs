using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAction : MonoBehaviour
{
    MainMachine machineScript;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void MachineInteract()
    {
        machineScript.PlayerInteraction();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (machineScript)
                MachineInteract();
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.tag == "Machine")
            machineScript = other.gameObject.GetComponent<MainMachine>();
    }

    private void OnCollisionExit2D(Collision2D other) {
        if (other.gameObject.tag == "Machine")
            machineScript = null;
    }
}
