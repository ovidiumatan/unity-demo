using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public int speed = 50;
    public int jumpParam = 40;
    private Rigidbody2D rigidBody2DComponent;
    private Vector2 movement;
    bool isJumping = false;
    private Animator animator;
    const string animatorStateString = "PlayerState";
    public AudioSource backgroundMusic;
    public AudioSource runSound;
    public AudioSource jumpSound;
    private AnimationState state;
    // Start is called before the first frame update

    enum AnimationState
    {
        idle=0,
        run=1,
        jump=2
    }
    void Start()
    {
        rigidBody2DComponent = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        movement = new Vector2();
        backgroundMusic.Play();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        isJumping = false;

    }

    public void OnCollisionExit2D(Collision2D collision)
    {
        isJumping = true;

    }



    private void FixedUpdate()
    {
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");
        movement.Normalize();

        AnimationState currentState = getCurrentState();
        animator.SetInteger(animatorStateString, (int)currentState);

        playSounds(currentState);
        state = currentState;

        if (Input.GetKey(KeyCode.Q) && (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))) Application.Quit();


        rigidBody2DComponent.velocity = new Vector2(movement.x * Time.deltaTime * speed, rigidBody2DComponent.velocity.y);

        if (movement.y != 0 && !isJumping)
        {
            rigidBody2DComponent.AddForce(Vector2.up * jumpParam);
        }
    }

    private void playSounds(AnimationState currentState)
    {
        if (currentState != state)
        {
            AudioSource previousAudio = getSoundSrcForState(state);
            if (previousAudio) previousAudio.Stop();
            AudioSource currentAudio = getSoundSrcForState(currentState);
            if (currentAudio) currentAudio.Play();
        }
    }

    private AnimationState getCurrentState()
    {
        AnimationState state;
        if (isJumping) state = AnimationState.jump;
        else if (rigidBody2DComponent.velocity.x != 0) state = AnimationState.run;
        else state=AnimationState.idle;
        
        return state;
    }

    private AudioSource getSoundSrcForState(AnimationState state)
    {
        AudioSource res=null;
        switch (state) {
        case AnimationState.run:
                res = runSound; break;
            case AnimationState.jump:
                res = jumpSound; break;
        }
        return res;
    }
}
