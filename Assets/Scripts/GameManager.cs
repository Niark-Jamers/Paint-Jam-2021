using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public string[] sceneList;

    string curScene;
    bool pause = false;
    int sceneNumber;

    public static GameManager instance;

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
        SceneManager.LoadScene(sceneList[sceneNumber + 1]);
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
