using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class brokenQTE : MonoBehaviour
{
    public Sprite leftSolution;
    public Sprite rightSolution;
    public Sprite downSolution;
    public Sprite upSolution;
    public int iteration = 3;
    private List<Sprite> possibleSolution = new List<Sprite>();
    private int solutionNumber;
    public GameObject problem;
    // Start is called before the first frame update
    void Start()
    {
        possibleSolution.Add(leftSolution);
        possibleSolution.Add(rightSolution);
        possibleSolution.Add(downSolution);
        possibleSolution.Add(upSolution);
        problem.gameObject.GetComponent<Image>().sprite = ChooseRandom();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            TrySolution(0);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            TrySolution(1);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            TrySolution(2);
        }
        if (Input.GetKey(KeyCode.UpArrow))
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
            iteration -= 1;
            Debug.Log(iteration);
            GoodSolution();
        }
        else
        {
            WrongSolution();
        }
    }

    void GoodSolution()
    {
        if (iteration > 0)
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
        gameObject.SetActive(false);
    }
}

