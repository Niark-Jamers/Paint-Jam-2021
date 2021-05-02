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

    public Animator fadeInAnimation;
    public float fadeInTime = 1f;
    public Animator fadeOutAnimation;
    public float fadeOutTime = 2f;


    // Start is called before the first frame update
    void Start()
    {
        curScene = SceneManager.GetActiveScene().name;
        instance = this;

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
    }

    public void LoadNext()
    {
        Debug.Log(sceneNumber + " | " + fadeInAnimation);
        if (sceneNumber != 0 && fadeInAnimation != null)
        {
            StartCoroutine(FadeAndLoad(sceneList[sceneNumber + 1]));
        }
    }

    IEnumerator FadeAndLoad(string scene)
    {
        fadeInAnimation.SetTrigger("Start");

        // Wait animation finish
        float t = Time.time;
        while (Time.time - t < fadeInTime)
            yield return new WaitForEndOfFrame();
        Debug.Log("NOPE!");
        
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

    public void Restart()
    {
        SceneManager.LoadScene(curScene);
    }

    void Pause()
    {
        pause = !pause;
    }
}
