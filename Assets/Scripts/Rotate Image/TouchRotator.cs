using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchRotator : MonoBehaviour
{
    private void OnMouseDown()
    {
        if (!GameObject.FindGameObjectsWithTag("RotateManager")[0].GetComponent<RotateGame>().gameComplete)
        {
            transform.Rotate(0f, 0f, 90f);
        }
    }
}
