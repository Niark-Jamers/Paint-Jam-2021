using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentBrokenGame : MonoBehaviour
{
    public MainMachine machine;

    public GameObject VentUI;

    public GameObject manivelle;
    public GameObject upKey;
    public GameObject rightKey;
    public GameObject downKey;
    public GameObject leftKey;

    public AudioClip[] woosh = new AudioClip[2];

    [HideInInspector] public enum VentState {up, right, down, left};

    int enumLenght;

    VentState currentState;

    int victoryCount = 8;
    int winCount;

    private void OnEnable() 
    {
        VentUI.SetActive(true);
        winCount = 0;
        enumLenght =  System.Enum.GetValues(typeof(VentState)).Length;
        currentState = (VentState)Random.Range(0, enumLenght);
        Debug.Log(currentState.ToString());
        SetManivelleDir();
        activateKey();
    }

    void SetManivelleDir()
    {
        manivelle.transform.rotation = Quaternion.Euler(0, 0, (int)currentState * -90);
    }

    void activateKey()
    {
        upKey.SetActive(false);
        rightKey.SetActive(false);
        downKey.SetActive(false);
        leftKey.SetActive(false);
        switch (currentState){
            case VentState.up :
            {
                upKey.SetActive(true);
                break;
            }
            case VentState.right :
            {
                rightKey.SetActive(true);
                break;
            }
            case VentState.down :
            {
                downKey.SetActive(true);
                break;
            }
            case VentState.left :
            {
                leftKey.SetActive(true);
                break;
            }
            default:
                break;
        }
    }

    void NextKey()
    {
        AudioManager.instance.PlaySFX(woosh[Random.Range(0, 2)], 0.3f);
        currentState = (VentState)(((int)currentState + 1) < enumLenght? ((int)currentState + 1): 0);
        SetManivelleDir();
        activateKey();
        winCount += 1;
    }

    private void Update() 
    {
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.UpArrow)) && currentState == VentState.up)
            NextKey();
        else if ((Input.GetKeyDown(KeyCode.D)|| Input.GetKeyDown(KeyCode.RightArrow)) && currentState == VentState.right)
            NextKey();
        else if ((Input.GetKeyDown(KeyCode.S)|| Input.GetKeyDown(KeyCode.DownArrow)) && currentState == VentState.down)
            NextKey();
        else if ((Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && currentState == VentState.left)
            NextKey();

        if (winCount > victoryCount)
        {
            machine.BrokenStop();
            machine.CloseBrokenGame();
            VentUI.gameObject.SetActive(false);
        }
    }
}
