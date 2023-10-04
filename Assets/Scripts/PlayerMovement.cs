using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public float jumpSpeed = 8f;
    private float direction = 0f;
    public bool canMove = true;
    public float xRangeL = 8.56f;
    public float xRangeR = 8.56f;
    private Rigidbody2D player;

    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;
    private bool isTouchingGround;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        isTouchingGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        direction = Input.GetAxis("Horizontal");

        if (transform.position.x < -xRangeL)
        {
            transform.position = new Vector3(-xRangeL, transform.position.y, transform.position.z);
        }
        if (transform.position.x > xRangeR)
        {
            transform.position = new Vector3(xRangeR, transform.position.y, transform.position.z);
        }

        if (canMove)
        {
            if (direction != 0f)
            {
                player.velocity = new Vector2(direction * speed, player.velocity.y);
            }
            else
            {
                player.velocity = new Vector2(0, player.velocity.y);
            }

            if (Input.GetButtonDown("Jump") && isTouchingGround)
            {
                player.velocity = new Vector2(player.velocity.x, jumpSpeed);
            }
        }
    }
}
