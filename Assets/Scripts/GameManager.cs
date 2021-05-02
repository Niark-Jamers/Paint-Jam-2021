using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public string[] sceneList;

    string curScene;
    bool pause = false;
    int sceneNumber;

    public static GameManager instance;
    
    [Header("Level settings")]
    public int canGoal = 30;
    public Slider canSlider;
    public float maxTimeInLevel = 60;

    float levelStartTime;

    [Header("Animations")]
    public Animator fadeInAnimation;
    public float fadeInTime = 1f;
    public Animator fadeOutAnimation;
    public float fadeOutTime = 2f;
    public Animator fadeInAnimationFailed;
    public float fadeOutTimeFailed = 2f;
    public Animator fadeGameOver;

    bool levelSucceeded = false;

    [Header("Features")]
    public bool canCatchFire = true;
    public bool electricityCanBreak = true;
    public float electricityOutageTimeout = 30;
    float electricityBreakTime;
    float electricityOutageTimeoutWithRandom;

    internal bool electricityBroken;

    GameObject[] lights;

    // Start is called before the first frame update
    void Start()
    {
        levelStartTime = Time.time;
        electricityBreakTime = Time.time;
        electricityOutageTimeoutWithRandom = electricityOutageTimeout + Random.Range(-15f, 15f);

        lights = Object.FindObjectsOfType<PolygonCollider2D>().Select(p => (p as PolygonCollider2D).gameObject).ToArray();

        curScene = SceneManager.GetActiveScene().name;
        instance = this;

        if (canSlider != null)
        {
            canSlider.maxValue = canGoal;
            canSlider.value = 0;
        }

        for (int i = 0; i < sceneList.Length; i++)
        {
            if (sceneList[i] == curScene)
            {
                sceneNumber = i;
                break;
            }
        }

        if (sceneNumber != 0 && fadeOutAnimation != null)
        {
            StartCoroutine(FadeAfterLoad());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Pause();
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            LoadNext();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }

        if (GetLevelTimeBetween01() >= 1.0f)
        {
            if (levelSucceeded)
            {
                StartCoroutine(FinishLevel());

                IEnumerator FinishLevel()
                {
                    // TODO: play sound and wait a bit
                    yield return new WaitForSeconds(1);

                    yield return FadeAndLoad(sceneList[sceneNumber + 1]);
                }
            }
            else
            {
                StartCoroutine(RestartLevel());

                IEnumerator RestartLevel()
                {
                    // TODO: play you loose music
                    yield return new WaitForSeconds(1f);

                    yield return FadeAndLoad(sceneList[sceneNumber]);
                }
            }
        }

        if (electricityCanBreak)
        {
            if (Time.time - electricityBreakTime > electricityOutageTimeoutWithRandom && !electricityBroken)
            {
                electricityOutageTimeoutWithRandom = electricityOutageTimeout + Random.Range(-15f, 15f);
                BreakElectricity();
            }
        }
    }

    public void LoadNext()
    {
        if (sceneNumber != 0 && fadeInAnimation != null)
        {
            StartCoroutine(FadeAndLoad(sceneList[sceneNumber + 1]));
        }
        else
            SceneManager.LoadScene(sceneList[sceneNumber + 1]);
    }

    IEnumerator FadeAndLoad(string scene)
    {
        AudioManager.instance.StopBackgroundMusic();
        if (levelSucceeded)
            fadeInAnimation.SetTrigger("StartSuccess");
        else
            fadeInAnimationFailed.SetTrigger("StartFail");

        // Wait animation finish
        float t = Time.time;
        while (Time.time - t < fadeInTime)
            yield return new WaitForEndOfFrame();
        
        SceneManager.LoadScene(scene);
    }

    IEnumerator FadeAfterLoad()
    {
        fadeOutAnimation.SetTrigger("Start");
    
        // Wait animation finish
        float t = Time.time;
        while (Time.time - t < fadeOutTime)
            yield return new WaitForEndOfFrame();
 
        yield break;
    }

    public void GameOverAnimation()
    {
        fadeGameOver.SetTrigger("Start");
    }

    public void AddCans(int amount)
    {
        canSlider.value += amount;

        if (canSlider.value == canGoal)
        {
            Debug.Log("OK!");
            levelSucceeded = true;
        }
    }

    public float GetLevelTimeBetween01() => Mathf.Clamp01((Time.time - levelStartTime) / maxTimeInLevel);

    public void Restart()
    {
        SceneManager.LoadScene(curScene);
    }

    public void BreakElectricity()
    {
        electricityBroken = true;

        // Disable lights:
        foreach (var light in lights)
            light.SetActive(false);
    }

    public void RepairElectricity()
    {
        electricityBroken = false;
        electricityBreakTime = Time.time;

        // Disable lights:
        foreach (var light in lights)
            light.SetActive(true);
    }

    void Pause()
    {
        pause = !pause;
    }
}
