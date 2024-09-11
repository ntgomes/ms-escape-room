using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractionManager : MonoBehaviour
{
    public System.Action[] eventArray; // Effectively an array of void function pointers that do not accept parameters
    
    public bool slidingBlockComplete;
    public bool memoryCardComplete;
    public bool rotatePuzzleComplete;

    public bool monkeyBallComplete;

    public bool hasKey = false;
    
    private TextBox textBoxInstance;

    // Start is called before the first frame update
    void Start()
    {
        textBoxInstance = GameObject.FindGameObjectsWithTag("TextManager")[0].GetComponent<TextBox>();
        eventArray = new System.Action[] { 
            FridgeEvent, DoorEvent, EndSlidingBlock1, EndSlidingBlock2, 
            StartSlidingBlock, GoToEnding, EndingSequence1, EndingSequence2, 
            QuitGame, TableEvent, StartMonkeyBall, EndMonkeyBall1, EndMonkeyBall2,
            EndRotatePuzzle1, EndRotatePuzzle2, DresserEvent, StartRotatePuzzle
        };

        slidingBlockComplete = PlayerPrefs.GetInt("SBComplete") == 1;
        monkeyBallComplete = PlayerPrefs.GetInt("MBComplete") == 1;
        rotatePuzzleComplete = PlayerPrefs.GetInt("RPComplete") == 1;

        hasKey = slidingBlockComplete && rotatePuzzleComplete && monkeyBallComplete;
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void FireEvent(int eventIdx)
    {
        if (!textBoxInstance.textDisplaying)
        {
            eventArray[eventIdx]();
        }
    }

    public void FridgeEvent()
    {
        if (!slidingBlockComplete)
        {
            textBoxInstance.SetText(
                "None of the fridge's doors are able to be opened.|" +
                "You see a sliding block puzzle, which may solve that issue.|" +
                "However, just as you begin solving the puzzle, you start to experience blurred vision..."
            , 4);
        }
        else
        {
            textBoxInstance.SetText(
                "You have completed the sliding block puzzle, from which you got|" +
                "eye drops and a key piece."
            );
        }
    }

    public void TableEvent()
    {
        if (!monkeyBallComplete)
        {
            textBoxInstance.SetText(
                "You see a marble tilt puzzle. Maybe completing it will help you get out.|" +
                "You feel lightheaded and dizzy as you start, making it hard to balance the marble..." 
            , 10);
        }
        else
        {
            textBoxInstance.SetText(
                "You have completed the marble puzzle, from which you got a bottle of Meclizine|" +
                "and a key piece."
            );
        }
    }

    public void DresserEvent()
    {
        if (!rotatePuzzleComplete)
        {
            textBoxInstance.SetText(
                "This dresser has a rotating image puzzle attached to one of the drawers. |" +
                "Completing it should unlock the drawer and give you something useful.|" +
                "However, you begin losing coordination and balance as you begin the puzzle."
            , 16);
        }
        else
        {
            textBoxInstance.SetText(
                "You have completed the rotating image puzzle, from which you got a bottle of|" +
                "Antivert and a key piece."
            );
        }
    }

    public void DoorEvent()
    {
        if (!hasKey)
        {
            textBoxInstance.SetText(
                "The door is locked. You will need to find a key."
            );
        } 
        else
        {
            textBoxInstance.SetText(
                "Using all the key pieces you gathered, you combine them together and unlock the door!"
            , 5);
        }
    }

    public void EndSlidingBlock1()
    {
        textBoxInstance.SetText(
            "A voice echos from an unknown place in the locked room...|" +
            "\"When I suffered from blurry vision, you were one of the classmates who made fun of me.|" +
            "You never once thought about how disoriented or frustrated I felt...|" +
            "I hope you now understand those same feelings from solving this puzzle.\""
        , 3);
    }

    public void EndSlidingBlock2()
    {
        PlayerPrefs.SetInt("SBComplete", 1);
        SceneManager.LoadScene("Escape Room");
    }

    public void StartSlidingBlock()
    {
        PlayerPrefs.SetFloat("PlayerPos", GameObject.FindGameObjectsWithTag("Player")[0].transform.position.x);
        SceneManager.LoadScene("Sliding Block");
    }

    public void EndMonkeyBall1()
    {
        textBoxInstance.SetText(
            "You hear a voice coming from the locked room...|" +
            "\"Vertigo was a symptom I began experiencing later on while in school.|" +
            "I would lose a lot of coordination from trying to balance on various things. |" +
            "You and your friends always mocked me as I struggled with doing the simple things...|" +
            "I hope you now understand those same feelings from solving this puzzle.\""
        , 12);
    }

    public void EndMonkeyBall2()
    {
        PlayerPrefs.SetInt("MBComplete", 1);
        SceneManager.LoadScene("Escape Room");
    }

    public void EndRotatePuzzle1()
    {
        textBoxInstance.SetText(
            "You hear a voice coming from the locked room...|" +
            "\"The teachers told you that my name is Woodley.|" +
            "They told you that I had a disorder called Multiple Sclerosis.|" +
            "They told you that I would sometimes get very dizzy when standing and walking around.|" +
            "It affected me when it came to simple puzzles like this.|" +
            "But you certainly didn't seem to care about that all those years ago...|" +
            "Those times where my dizziness caused me to bump into you... and how much you yelled at me. |" +
            "I hope you now understand those same feelings from solving this puzzle.\""
        , 14);
    }

    public void EndRotatePuzzle2()
    {
        PlayerPrefs.SetInt("RPComplete", 1);
        SceneManager.LoadScene("Escape Room");
    }

    public void StartMonkeyBall()
    {
        PlayerPrefs.SetFloat("PlayerPos", GameObject.FindGameObjectsWithTag("Player")[0].transform.position.x);
        SceneManager.LoadScene("Monkey Ball");
    }

    public void StartRotatePuzzle()
    {
        PlayerPrefs.SetFloat("PlayerPos", GameObject.FindGameObjectsWithTag("Player")[0].transform.position.x);
        SceneManager.LoadScene("Rotate Image");
    }

    public void GoToEnding()
    {
        SceneManager.LoadScene("Ending");
    }

    public void EndingSequence1()
    {
        textBoxInstance.SetText(
            "As you return to the outside world, you feel the impact that you had on Woodley's life. |" +
            "You realize that despite being locked in a room, Woodley ultimately wanted to help you |" +
            "as you began having symptoms of multiple sclerosis.|" +
            "The eye drops for the blurry vision, the Meclizine for the vertigo |" +
            "and the Antivert for the general dizziness.  They can help with those symptoms. |" +
            "Perhaps the puzzles in that room, and the troubles you had with it |" +
            "will help you understand just how life-changing MS can be. "
        , 7);
    }

    public void EndingSequence2()
    {
        StartCoroutine(RollCredits());
    }

    IEnumerator RollCredits()
    {
        yield return new WaitForSeconds(1f);
        textBoxInstance.SetText(
            "MULTIPLE SCLEROSIS AND THE LOCKED ROOM |" +
            "Made by Neeloy Gomes and Luke Knudsen |" +
            "Learn more at www.nationalmssociety.org"
        , 8);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
