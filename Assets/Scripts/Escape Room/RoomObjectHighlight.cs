using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomObjectHighlight : MonoBehaviour
{
    public int eventIndex;

    private TextBox textBoxInstance;
    private bool highlighted = false;

    // Start is called before the first frame update
    void Start()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        textBoxInstance = GameObject.FindGameObjectsWithTag("TextManager")[0].GetComponent<TextBox>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.GetChild(0).gameObject.SetActive(highlighted);
        if (highlighted)
        {
            if (Input.GetButtonDown("Submit") && !textBoxInstance.textDisplaying)
            {
                GameObject.FindGameObjectsWithTag("EventSystem")[0].GetComponent<InteractionManager>().FireEvent(eventIndex);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            highlighted = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            highlighted = false;
        }
    }
}
