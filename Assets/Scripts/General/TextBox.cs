using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBox : MonoBehaviour
{
    public bool textDisplaying = false;
    public GameObject textBoxObject;
    public Text textField;

    private List<string> textsToDisplay = new List<string>();
    private int textIndex = 0;
    private bool keyPressed = false;
    private int postTextEventIndex = -1;

    // Start is called before the first frame update
    void Start()
    {
        textBoxObject.SetActive(false);
        // Here, get the child object of the text box that is the text field (normally, it's 0, but in some scenes it's not)
        for (int i = 0; i < textBoxObject.transform.GetChild(0).transform.childCount; i++)
        {
            textField = textBoxObject.transform.GetChild(0).transform.GetChild(i).GetComponent<Text>();
            if (textField != null)
            {
                break;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (textDisplaying)
        {
            textField.text = textsToDisplay[textIndex];
            if (Input.GetButtonDown("Submit") && !keyPressed)
            {
                if (textIndex == textsToDisplay.Count - 1)
                {
                    StartCoroutine(EndText());
                } 
                else
                {
                    StartCoroutine(ProgressText());
                }
            }
        }
    }

    public void SetText(string fullText)
    {
        textsToDisplay.Clear();
        postTextEventIndex = 0;
        string[] splitStrings = fullText.Split('|');

        foreach(string splitString in splitStrings)
        {
            textsToDisplay.Add(splitString);
        }
        textDisplaying = true;
        textIndex = 0;
        textBoxObject.SetActive(true);
    }

    public void SetText(string fullText, int postTextEventIdx)
    {
        SetText(fullText);
        postTextEventIndex = postTextEventIdx;
    }

    // https://stackoverflow.com/a/4133475
    private IEnumerable<string> SplitInParts(string s, int partLength)
    {
        for (var i = 0; i < s.Length; i += partLength)
            yield return s.Substring(i, System.Math.Min(partLength, s.Length - i));
    }
    
    IEnumerator ProgressText()
    {
        keyPressed = true;
        textIndex += 1;
        yield return new WaitForSeconds(0.1f);

        keyPressed = false;
    }

    IEnumerator EndText()
    {
        keyPressed = true;
        yield return new WaitForSeconds(0.1f);
        keyPressed = false;
        textDisplaying = false;
        textBoxObject.SetActive(false);

        if (postTextEventIndex > 0)
        {
            GameObject.FindGameObjectsWithTag("EventSystem")[0].GetComponent<InteractionManager>().FireEvent(postTextEventIndex);
        }
    }
}
