using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DoTheEnding());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator DoTheEnding()
    {
        yield return new WaitForSeconds(2f);
        GameObject.FindGameObjectsWithTag("EventSystem")[0].GetComponent<InteractionManager>().FireEvent(6);
    }
}
