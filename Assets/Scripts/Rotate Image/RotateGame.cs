using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateGame : MonoBehaviour
{
    [SerializeField]
    private Transform[] pictures;

    public bool gameComplete;

    private bool textBoxCompleted = false;

    void Start()
    {
        gameComplete = false;
        // Scramble the picture rotations
        foreach (Transform picture in pictures)
        {
            int angleChoice = Random.Range(0, 3);
            float angleToRotate = 0f;
            switch(angleChoice)
            {
                case 0:
                    angleToRotate = 90f;
                    break;
                case 1:
                    angleToRotate = 180f;
                    break;
                case 2:
                    angleToRotate = 270f;
                    break;
            }
            picture.Rotate(0, 0, angleToRotate);
        }
    }

    void Update()
    {
        if (gameComplete)
        {
            if (textBoxCompleted)
            {
                return;
            }
            else
            {
                GameObject.FindGameObjectsWithTag("Music")[0].GetComponent<AudioSource>().Stop();
                StartCoroutine(EndGame());
            }
        }
        int correctPictures = 0;
        foreach (Transform picture in pictures)
        {
            if (picture.rotation.z < 0.01f)
            {
                correctPictures++;
            } else
            {
                correctPictures--;
            }
        }
        if (correctPictures == pictures.Length)
        {
            gameComplete = true;
        }
    }

    IEnumerator EndGame()
    {
        textBoxCompleted = true;
        yield return new WaitForSeconds(2f);
        GameObject.FindGameObjectsWithTag("EventSystem")[0].GetComponent<InteractionManager>().FireEvent(13);
    }
}
