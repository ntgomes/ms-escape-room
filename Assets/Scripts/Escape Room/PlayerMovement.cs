using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5.0f;

    private Vector3 moveVector;
    private Animator playerAnimator;
    private float startingPlayerScale; // Should be positive
    private float startingPlayerY;
    private TextBox textBoxInstance;

    const string animationState = "PlayerAnimState";

    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        startingPlayerScale = transform.localScale.x;
        startingPlayerY = transform.position.y;
        textBoxInstance = GameObject.FindGameObjectsWithTag("TextManager")[0].GetComponent<TextBox>();

        transform.position = new Vector3(PlayerPrefs.GetFloat("PlayerPos", transform.position.x), transform.position.y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        bool isMoving = false;
        float xDelta = 0.0f;
        if (!textBoxInstance.textDisplaying)
        {
            xDelta = Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime;
        }
        if (xDelta != 0.0f)
        {
            if (xDelta < 0.0f && transform.localScale.x > 0)
            {
                transform.localScale = new Vector3(-1 * startingPlayerScale, transform.localScale.y, transform.localScale.z);
            }
            if (xDelta > 0.0f && transform.localScale.x < 0)
            {
                transform.localScale = new Vector3(startingPlayerScale, transform.localScale.y, transform.localScale.z);
            }
            playerAnimator.SetInteger(animationState, 1);
            isMoving = true;
        } 
        else
        {
            playerAnimator.SetInteger(animationState, 0);
        }
        transform.position = new Vector3(
            transform.position.x + xDelta, 
            // Changing y based on whether or not the character is moving due to the way Unity clipped the animation box...
            // The more elegant solution is to just have the animation sprites match up to the idle sprite;
            // Unfortunately, that takes too much time to adjust, so we'll just have to do with covering up the change in y that occurs
            isMoving ? startingPlayerY - 0.2f : startingPlayerY, 
            transform.position.z
        );
    }
}
