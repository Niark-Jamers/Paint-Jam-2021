using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMachine : MonoBehaviour
{
    [HideInInspector] public enum State {Working, Broken, Fire};
    public State currentState = State.Working;

    public GameObject interactionKey;

    [Header("Minigames")]
    public GameObject workingMiniGame;
    public GameObject brokenMiniGame;
    public GameObject fireMiniGame;

    [Header("Other")]
    public GameObject firePrefab;

    CharacterController2D character;

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
        character = FindObjectOfType<CharacterController2D>();
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
        firePrefab.SetActive(true);
        sr.color = Color.red;
    }

    public void FireStop()
    {
        fireBar = 0;
        currentState = State.Working;
        interactionKey.SetActive(false);
        firePrefab.SetActive(false);
        sr.color = Color.white;
    }

    public void PlayerInteraction()
    {
        switch (currentState)
        {
            case State.Broken :
            {
                StartBrokenGame();
                break;
            }
            case State.Fire:
            {
                StartFireGame();
                break;
            }
            default:
            {
                StartWorkingGame();
                break;
            }
        }
    }

    public void StartWorkingGame()
    {
        if (workingMiniGame)
        {
            workingMiniGame.SetActive(true);
            interactionKey.SetActive(false);
            character.disableInputs = true;
        }
    }

    public void StartBrokenGame()
    {
        brokenMiniGame.SetActive(true);
        interactionKey.SetActive(false);
        character.disableInputs = true;
    }

    public void StartFireGame()
    {
        if (fireMiniGame)
        {
            fireMiniGame.SetActive(true);
            interactionKey?.SetActive(false);
            character.disableInputs = true;
        }
    }

    public void CloseWorkingGame()
    {
        workingMiniGame.SetActive(false);
        character.disableInputs = false;
    }

    public void CloseBrokenGame()
    {
        brokenMiniGame.SetActive(false);
        character.disableInputs = false;
    }

    public void CloseFireGame()
    {
        fireMiniGame.SetActive(false);
        character.disableInputs = false;
    }

    void Update()
    {
        trueTimer += Time.deltaTime;
        if (trueTimer >= badtimer)
        {
            if (currentState == State.Working && (workingMiniGame == null || !workingMiniGame.activeSelf))
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

    void CloseGame()
    {
        interactionKey.SetActive(false);
    }

    private void OnTriggerStay2D(Collider2D other) {
        if (other.gameObject.tag == "Player" && currentState != State.Working && !interactionKey.activeSelf && (!brokenMiniGame.activeSelf && !fireMiniGame.activeSelf))
        {
            interactionKey.SetActive(true);
        }
    }


    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.tag == "Player")
        {
            CloseGame();
        }
    }
}
