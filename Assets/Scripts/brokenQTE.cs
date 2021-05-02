using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class brokenQTE : MonoBehaviour
{
    public MainMachine machine;
    public AudioSource goodBoySound;
    public AudioSource badBoySound;
    public Sprite leftSolution;
    public Sprite rightSolution;
    public Sprite downSolution;
    public Sprite upSolution;
    public int iteration = 3;
    private int curIteration = 3;
    private List<Sprite> possibleSolution = new List<Sprite>();
    private int solutionNumber;
    public GameObject problem;
    // Start is called before the first frame update
    void Start()
    {
        curIteration = iteration;
        possibleSolution.Add(leftSolution);
        possibleSolution.Add(rightSolution);
        possibleSolution.Add(downSolution);
        possibleSolution.Add(upSolution);
        problem.gameObject.GetComponent<Image>().sprite = ChooseRandom();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.Q))
        {
            TrySolution(0);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            TrySolution(1);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            TrySolution(2);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Z))
        {
            TrySolution(3);
        }
    }

    Sprite ChooseRandom()
    {
        solutionNumber = Random.Range(0, 3);
        problem.gameObject.GetComponent<Image>().sprite = possibleSolution[solutionNumber];
        return possibleSolution[solutionNumber];
        
    }

    void TrySolution(int trying)
    {
        if (trying == solutionNumber)
        {
            curIteration -= 1;
            GoodSolution();
        }
        else
        {
            badBoySound.Play();
            WrongSolution();
        }
    }

    void GoodSolution()
    {
        goodBoySound.Play();
        if (curIteration > 0)
        {
            ChooseRandom();
        }
        else
        {
            QuitMiniGame();
        }
    }
    void WrongSolution()
    {

    }
    void QuitMiniGame()
    {
        machine.BrokenStop();
        StartCoroutine(Exite());
    }

    IEnumerator Exite()
    {
        yield return new WaitWhile(() => goodBoySound.isPlaying);
        machine.CloseBrokenGame();
    }
}

