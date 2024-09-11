using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DizzinessEffect : MonoBehaviour
{
    /* Intended for use with the main camera object only */

    private int currentDir = 0;

    void Start()
    {
        currentDir = 1;
    }

    void Update()
    {
        if (GameObject.FindGameObjectsWithTag("RotateManager")[0].GetComponent<RotateGame>().gameComplete)
        {
            transform.Rotate(0f, 0f, -1 * transform.rotation.z);
            return;
        }
        if (currentDir == 1 && transform.rotation.z > 0.1f)
        {
            currentDir = -1;
        }
        else if (currentDir == -1 && transform.rotation.z < -0.1f)
        {
            currentDir = 1;
        }
        transform.Rotate(0f, 0f, currentDir * 4.0f * Time.deltaTime);
    }
}
