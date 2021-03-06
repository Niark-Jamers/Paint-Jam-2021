using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMachine : MonoBehaviour
{
    [HideInInspector] public enum State {Working, Broken, Fire};
    public State currentState = State.Working;

    public GameObject interactionKey;

    [Header("Colors")]
    public Color fireColor;
    public Color brokenColor;

    [Header("Sprites")]
    public Sprite brokenSprite;
    Sprite workingSprite;

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
    public float brokenStepOffset = 10;

    public float fireBar = 0;
    public float fireStep = 10;
    public float fireStepOffset = 6;

    public float shakeStep = 1.0f;


    public float canProductionTimeout = 1.5f;
    float lastCanProductionTime;

    internal bool isInFire, isBroken;
    Vector3 startingPos;

    SpriteRenderer sr;

    void Start()
    {
        lastCanProductionTime = Time.time;
        interactionKey.SetActive(false);
        sr = this.GetComponent<SpriteRenderer>();
        character = FindObjectOfType<CharacterController2D>();
        workingSprite = sr.sprite;
        startingPos = transform.position;
    }

    public void BrokenStart()
    {
        isBroken = true;
        currentState = State.Broken;
        sr.color = brokenColor;
        sr.sprite = brokenSprite;
    }

    public void BrokenStop()
    {
        brokenBar = 0;
        isBroken = false;
        currentState = State.Working;
        interactionKey.SetActive(false);
        sr.color = Color.white;
        sr.sprite = workingSprite;
    }

    public void FireStart()
    {
        isInFire = true;
        currentState = State.Fire;
        firePrefab.SetActive(true);
        sr.color = fireColor;
    }

    public void FireStop()
    {
        isInFire = false;
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

    bool isWorking =>!isBroken && !isInFire && !GameManager.instance.electricityBroken;

    void Update()
    {
        if (isWorking)
            transform.position = Vector3.MoveTowards(transform.position, startingPos + Random.insideUnitSphere, shakeStep * Time.deltaTime);
        trueTimer += Time.deltaTime;
        if (trueTimer >= badtimer)
        {
            if (currentState == State.Working && (workingMiniGame == null || !workingMiniGame.activeSelf))
            {
                brokenBar += brokenStep + Random.Range(-brokenStepOffset, brokenStepOffset);
                if (GameManager.instance.canCatchFire)
                    fireBar += fireStep + Random.Range(-fireStepOffset, fireStepOffset);
            }
            trueTimer = 0;
        }
        if (fireBar >= 100)
            FireStart();
        if (brokenBar >= 100)
            BrokenStart();

        if (isWorking)
        {
            // Produce cans:
            if (Time.time - lastCanProductionTime > canProductionTimeout)
            {
                lastCanProductionTime = Time.time;
                GameManager.instance.AddCans(1);
            }
        }
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
