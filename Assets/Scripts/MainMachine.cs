using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMachine : MonoBehaviour
{
    [HideInInspector] public enum State {Working, Broken, Fire};
    public State currentState = State.Working;

    public float brokenBar = 0;
    public float brokenStep = 10;

    public float fireBar = 0;
    public float fireStep = 10;

    SpriteRenderer sr;

    void Start()
    {
        sr = this.GetComponent<SpriteRenderer>();
    }

    public void BrokenStart()
    {
        currentState = State.Broken;
        sr.color = Color.blue;
        // change sprite;
    }

    public void BrokenStop()
    {
        brokenBar = 0;
        currentState = State.Working;
        sr.color = Color.white;
    }

    public void FireStart()
    {
        currentState = State.Fire;
        sr.color = Color.red;
    }

    public void FireStop()
    {
        fireBar = 0;
        currentState = State.Working;
        sr.color = Color.white;
    }

    public void PlayerInteraction()
    {
        switch (currentState)
        {
            case State.Broken :
                {
                    MachineBrokenGame();
                    break;
                }
            case State.Fire:
                {
                    MachineFireGame();
                    break;
                }
            default:
                break;
        }
    }

    public virtual void MachineBrokenGame()
    {
        Debug.Log("broken game");
    }

    public virtual void MachineFireGame()
    {
        Debug.Log("fire game");
    }

    void Update()
    {
        if (fireBar >= 100)
            FireStart();
        if (brokenBar >= 100)
            BrokenStart();
    }


}
