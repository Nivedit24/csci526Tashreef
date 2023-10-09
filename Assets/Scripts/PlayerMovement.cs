using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 20f;
    public float jumpSpeed = 8f;
    private float direction = 0f;
    public float hoverSpeedFactor = 4f;
    public float hoverGravityFactor = 0.1f;
    public float hoverJumpFactor = 1.5f;
    public bool canMove = true;
    private Rigidbody2D player;
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;
    private bool isTouchingGround;
    public GameObject airBall;
    public float hoverTime;
    private CheckPoint checkPoint;
    private State currState;
    private DateTime startHoverTime;


    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Rigidbody2D>();
        checkPoint = new CheckPoint(transform);
        currState = State.Normal;
    }

    // Update is called once per frame
    void Update()
    {
        isTouchingGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        direction = Input.GetAxis("Horizontal");

        if (canMove)
        {
            player.velocity = new Vector2(direction * speed, player.velocity.y);

            if (Input.GetButtonDown("Jump") && isTouchingGround)
            {
                player.velocity = new Vector2(player.velocity.x, jumpSpeed);
            }
        }

        switch (currState)
        {
            case State.Dead:
                player.transform.position = checkPoint.position;
                currState = State.Normal;
                return;
            case State.Normal:
                break;
            case State.Hover:
                TimeSpan span = DateTime.UtcNow - startHoverTime;
                if (span.TotalSeconds > hoverTime)
                {
                    DismountAirBall();
                }
                break;
            default:
                return;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.gameObject.tag)
        {
            case "CheckPoint":
                Debug.Log("Player reach the CheckPoint");
                Debug.Log("transform is : ", transform);
                Debug.Log("Checkpoint is " + checkPoint.position);
                checkPoint.SetCheckPoint(transform);
                other.gameObject.SetActive(false);
                break;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        var player = gameObject;
        var other = collision.gameObject;
        switch (collision.gameObject.tag)
        {
            case "DeathFloor":
                Debug.Log("Player is hit by Death Floor");
                currState = State.Dead;
                break;
            case "Airball":
                Debug.Log("Collision with hover ball");
                if(currState != State.Hover)
                {
                    HoverOnAirBall(collision);
                }
                else
                {
                    Destroy(collision.gameObject.transform.parent.gameObject);
                    startHoverTime = DateTime.UtcNow;
                }
                break;
            default:
                break;
        }
    }

    void HoverOnAirBall(Collision2D collision)
    {
        collision.gameObject.tag = "none";
        GameObject sphereParent = collision.gameObject.transform.parent.gameObject;
        sphereParent.transform.SetParent(transform);
        sphereParent.transform.localPosition = new Vector3(0f, -1.46f, 0f);
        Transform playerBody = transform.Find("Body");
        sphereParent.name = "HoverBall";
        Vector3 bodyPosition = playerBody.localPosition;
        bodyPosition.y += sphereParent.transform.localScale.y;
        playerBody.localPosition = bodyPosition;
        speed *= hoverSpeedFactor;
        jumpSpeed *= hoverJumpFactor;
        transform.GetComponent<Rigidbody2D>().gravityScale *= hoverGravityFactor;
        currState = State.Hover;
        startHoverTime = DateTime.UtcNow;
    }

    void DismountAirBall()
    {
        Transform hoverBall = transform.Find("HoverBall");
        Transform playerBody = transform.Find("Body");
        Vector3 bodyPosition = playerBody.localPosition;
        bodyPosition.y -= hoverBall.transform.localScale.y;
        Destroy(hoverBall.gameObject);
        playerBody.localPosition = bodyPosition;
        speed /= hoverSpeedFactor;
        jumpSpeed /= hoverJumpFactor;
        transform.GetComponent<Rigidbody2D>().gravityScale *= hoverGravityFactor;
        currState = State.Normal;
    }
}

internal class CheckPoint
{
    public Vector3 position;

    public CheckPoint(Transform transform)
    {
        position = transform.position;
        Debug.LogFormat("Initial CheckPoint Position: ", position);
    }

    public void SetCheckPoint(Transform transform)
    {
        position = transform.position;
        Debug.LogFormat("Current CheckPoint Position: ", position);
    }
}

internal enum State
{
    Normal, Hover, Dead, Gone
}