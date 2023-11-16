using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    Animator animator;
    public PlayerMovement playerMovement;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
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
            transform.localScale = new Vector2(1, 1);
        }
        else
        {
            transform.localScale = new Vector2(-1, 1);
        }
    }
}
