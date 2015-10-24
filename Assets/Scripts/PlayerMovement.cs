using UnityEngine;
using System.Collections;
using System;

public class PlayerMovement : MonoBehaviour
{
    public float MaxSpeed = 0.10f;

    private Animator anim;
    private Transform trans;
    public int player;
    
    private bool isMoving;

    private float axis_x;
    private float axis_y;
    
    private float input_x = 0;
    private float input_y = 1;

    private float look_x = 0;
    private float look_y = 1;

    public AudioSource notMovingSound;
    public AudioSource movingSound;

    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        trans = gameObject.GetComponent<Transform>();
    }

    void Update()
    {
        CalculateAxis();

        anim.SetBool("isMoving", isMoving);
        anim.SetFloat("input_x", input_x);
        anim.SetFloat("input_y", input_y);

        if (anim.GetBool("hit")) anim.SetBool("isMoving", false);
    }

    public void FixedUpdate()
    {
        // Do everything only then if not hit
        if (!anim.GetBool("hit"))
        {
            ChangeInputFromMultipleKeyPresses();
            ActualyChangingCoordinatesAccordingToInput();
            SetLookingDirection();
            ApplyMovementSound();
        }
    }

    private void CalculateAxis()
    {
        if (player == 1)
        {
            if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D)) axis_x = 0;
            else if (Input.GetKey(KeyCode.D)) axis_x = 1;
            else if (Input.GetKey(KeyCode.A)) axis_x = -1;
            else axis_x = 0;

            if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S)) axis_y = 0;
            else if (Input.GetKey(KeyCode.W)) axis_y = 1;
            else if (Input.GetKey(KeyCode.S)) axis_y = -1;
            else axis_y = 0;
        }
        if (player == 2)
        {
            if (Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.LeftArrow)) axis_x = 0;
            else if (Input.GetKey(KeyCode.RightArrow)) axis_x = 1;
            else if (Input.GetKey(KeyCode.LeftArrow)) axis_x = -1;
            else axis_x = 0;

            if (Input.GetKey(KeyCode.UpArrow) && Input.GetKey(KeyCode.DownArrow)) axis_y = 0;
            else if (Input.GetKey(KeyCode.UpArrow)) axis_y = 1;
            else if (Input.GetKey(KeyCode.DownArrow)) axis_y = -1;
            else axis_y = 0;
        }
    }

    private void ChangeInputFromMultipleKeyPresses()
    {
        // Movement changing when pressing keys for both directions
        if (axis_x != 0 && axis_y != 0)
        {
            if (input_x == 0)
            {
                input_x = axis_x;
                input_y = 0;
            }
            if (input_y == 0)
            {
                input_x = 0;
                input_y = axis_y;
            }
        }
        // If at least one key pressed
        else if (axis_x != 0 || axis_y != 0)
        {
            input_x = axis_x;
            input_y = axis_y;
        }
    }

    private void ActualyChangingCoordinatesAccordingToInput()
    {
        // Movement when pressing a key
        if (axis_x != 0 || axis_y != 0)
        {
            // Move object
            isMoving = true;
            trans.position += new Vector3(MaxSpeed * input_x, MaxSpeed * input_y, 0);

            // Align to cells
            if (input_x == 0)
            {
                trans.position = new Vector3(Mathf.Round(trans.position.x), trans.position.y, 0);
            }
            if (input_y == 0)
            {
                trans.position = new Vector3(trans.position.x, Mathf.Round(trans.position.y), 0);
            }
        }
        else
        {
            isMoving = false;
        }
    }

    private void SetLookingDirection()
    {
        look_x = input_x;
        look_y = input_y;
    }

    private void ApplyMovementSound()
    {
        if (player == 1)
        {
            // Sounds moving and not moving
            if (IsSomethingPressed() && !movingSound.isPlaying)
            {
                notMovingSound.Stop();
                movingSound.Play();
            }
            else if (!IsSomethingPressed() && !notMovingSound.isPlaying)
            {
                notMovingSound.Play();
                movingSound.Stop();
            }
        }
    }

    private bool IsSomethingPressed()
    {
        return Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) ||
            Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) ||
             Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.UpArrow);
    }


    // message receiver from "BulletTankDestroy" and "loadmap"
    public void ResetPosition()
    {
        if (player == 1)
        {
            trans.position = new Vector3(-4, -12, 0);
        }
        else if (player == 2)
        {
            trans.position = new Vector3(4, -12, 0);
        }
    }
}
