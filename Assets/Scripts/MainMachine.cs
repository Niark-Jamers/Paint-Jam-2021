using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMachine : MonoBehaviour
{
    [HideInInspector] public enum State {Working, Broken, Fire};
    public State currentState = State.Working;

    public GameObject interactionKey;

    public float badtimer = 1f;
    float trueTimer = 0f;

    public float brokenBar = 0;
    public float brokenStep = 20;

    public float fireBar = 0;
    public float fireStep = 10;

    SpriteRenderer sr;

    void Start()
    {
        interactionKey.SetActive(false);
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
        interactionKey.SetActive(false);
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
        interactionKey.SetActive(false);
        sr.color = Color.white;
    }

    public void PlayerInteraction()
    {
        Debug.Log("case test");
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
            {
                MachineWorkingGame();
                break;
            }
        }
    }

    public virtual void MachineWorkingGame()
    {
        Debug.Log("working game");
    }

    public virtual void MachineBrokenGame()
    {
        Debug.Log("broken game");
        BrokenStop();
    }

    public virtual void MachineFireGame()
    {
        Debug.Log("fire game");
        FireStop();
    }

    void Update()
    {
        trueTimer += Time.deltaTime;
        if (trueTimer >= badtimer)
        {
            if (currentState == State.Working)
            {
                brokenBar += brokenStep;
                fireBar += fireStep;
            }
            trueTimer = 0;
        }
        if (fireBar >= 100)
            FireStart();
        if (brokenBar >= 100)
            BrokenStart();
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.tag == "Player" && currentState != State.Working && !interactionKey.activeSelf)
        {
            interactionKey.SetActive(true);
        }
    }


    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.tag == "Player")
        {
            interactionKey.SetActive(false);
        }
    }
}
