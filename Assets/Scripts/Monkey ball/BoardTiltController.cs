using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BoardTiltController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject board;
    public float speed = 15.0f;

    private Vector3 tiltVector;

    private Camera cam;

    private GameObject goal;

    public GameObject ball;

    void Start()
    {
        board = GameObject.FindGameObjectWithTag("Player");
        cam = Camera.main;
        ball = GameObject.FindGameObjectWithTag("Ball");
        goal = GameObject.FindGameObjectWithTag("Goal");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (goal.GetComponent<GoalCollider>().done) {
            GameObject.FindGameObjectsWithTag("Music")[0].GetComponent<AudioSource>().Stop();
            GameObject.FindGameObjectsWithTag("EventSystem")[0].GetComponent<InteractionManager>().FireEvent(11);
        }
        Move();
        if (ball.transform.position.y < -20.0f && !goal.GetComponent<GoalCollider>().done) {
            ball.transform.position = new Vector3(6.8f, 11.42f, 0.41f);
            transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
            Rigidbody rigidbody = ball.GetComponent<Rigidbody>();
            rigidbody.velocity = Vector3.zero;
            rigidbody.angularVelocity = Vector3.zero;
        }

    }

    private void Move()
    {
        // Getting the direction to move through player input
        float hMove = Input.GetAxis("Horizontal");
        float vMove = Input.GetAxis("Vertical");
        float speed = 15.0f;

        // Get directions relative to camera
        Vector3 forward = cam.transform.forward;
        Vector3 right = cam.transform.right;

        // Project forward and right direction on the horizontal plane (not up and down), then
        // normalize to get magnitude of 1
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        // Set the direction for the player to move
        Vector3 dir = right * hMove + forward * vMove;

        // Set the direction's magnitude to 1 so that it does not interfere with the movement speed
        dir.Normalize();

        // Move the player by the direction multiplied by speed and delta time 
        transform.eulerAngles -= dir * speed * Time.deltaTime;

    }
}
