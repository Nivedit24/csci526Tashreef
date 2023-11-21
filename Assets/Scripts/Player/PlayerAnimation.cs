using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    Animator animator;
    public PlayerMovement playerMovement;
    private Transform playerBody;
    private bool isPlayerInvisible;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        playerBody = transform.Find("Body");
        isPlayerInvisible = false;
    }

    public void TogglePlayerVisibility()
    {
        isPlayerInvisible = !isPlayerInvisible;
    }

    public void SetPlayerInvisible(bool isInvisible)
    {
        isPlayerInvisible = isInvisible;
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

        if (isPlayerInvisible)
        {
            playerBody.localScale = new Vector2(0, 0);
        }
        else if (playerMovement.faceRight)
        {
            playerBody.localScale = new Vector2(12, 12);
        }
        else
        {
            playerBody.localScale = new Vector2(-12, 12);
        }
    }
}
