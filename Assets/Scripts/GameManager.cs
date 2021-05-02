using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    bool levelSucceeded = false;

    [Header("Features")]
    public bool canCatchFire = true;

    // Start is called before the first frame update
    void Start()
    {
        levelStartTime = Time.time;

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
        if (levelSucceeded)
            fadeInAnimation.SetTrigger("Start");
        else
            fadeInAnimationFailed.SetTrigger("Start");

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

    public void AddCans(int amount)
    {
        canSlider.value += amount;

        if (canSlider.value == canGoal)
        {
            levelSucceeded = true;
        }
    }

    public float GetLevelTimeBetween01() => Mathf.Clamp01((Time.time - levelStartTime) / maxTimeInLevel);

    public void Restart()
    {
        SceneManager.LoadScene(curScene);
    }

    void Pause()
    {
        pause = !pause;
    }
}
