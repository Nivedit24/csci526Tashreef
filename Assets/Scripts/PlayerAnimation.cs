using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    Animator animator;
    public PlayerMovement playerMovement;
    private Transform playerBody;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        playerBody = transform.Find("Body");
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 velocity = playerMovement.playerRB.velocity;
        animator.SetFloat("HSpeed", Mathf.Abs(velocity.x));
        animator.SetFloat("VSpeed", Mathf.Abs(velocity.y));
        animator.SetBool("Idle", velocity.magnitude < .5f);
        animator.SetBool("Jumping", velocity.y > 1f);
        animator.SetBool("Falling", velocity.y < -1f);
        animator.SetBool("Hovering", playerMovement.currState == State.Hover);
        
        if (playerMovement.faceRight)
        {
            playerBody.localScale = new Vector2(12, 12);
        }
        else
        {
            playerBody.localScale = new Vector2(-12, 12);
        }
    }
}
