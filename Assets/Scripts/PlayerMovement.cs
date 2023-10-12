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

    public bool canMove = true;
    private Rigidbody2D player;
    public float groundCheckRadius;
    public LayerMask groundLayer;
    private bool isTouchingGround;
    public float hoverSpeedFactor = 2f;
    public float hoverGravityFactor = 0.75f;
    public float hoverJumpFactor = 1.5f;
    public float hoverTime;
    private DateTime startHoverTime;

    private CheckPoint checkPoint;
    public static State currState;
    public DamageReceiver playerReceiver;

    public static bool analytics01Enabled = false;

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
        isTouchingGround = Physics2D.OverlapCircle(player.position, groundCheckRadius, groundLayer);
        direction = Input.GetAxis("Horizontal");

        if (canMove)
        {
            player.velocity = new Vector2(direction * speed, player.velocity.y);

            if (Input.GetButtonDown("Jump") && isTouchingGround)
            {
                player.AddForce(new Vector2(player.velocity.x, jumpSpeed), ForceMode2D.Impulse);
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
            case "Airball":
                Debug.Log("Collision with hover ball");
                if (currState != State.Hover)
                {
                    HoverOnAirBall(collision);
                }
                else
                {
                    Destroy(collision.gameObject.transform.parent.gameObject);
                    startHoverTime = DateTime.UtcNow;
                }
                break;
            case "Respawn":
                currState = State.Dead;
                break;
            default:
                break;
        }
    }
    
    void OnCollisionStay2D(Collision2D collision)
    {
        var player = gameObject;
        var other = collision.gameObject;
        switch (collision.gameObject.tag)
        {
            case "DeathFloor":
                Debug.Log("Player is hit by Death Floor");
                //currState = State.Dead;
                playerReceiver.TakeDamage(30);
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
        sphereParent.GetComponent<RotateAir>().startRotate = true;
        Vector3 bodyPosition = playerBody.localPosition;
        bodyPosition.y += sphereParent.transform.localScale.y;
        playerBody.localPosition = bodyPosition;
        speed *= hoverSpeedFactor;
        jumpSpeed *= hoverJumpFactor;
        transform.GetComponent<Rigidbody2D>().gravityScale *= hoverGravityFactor;
        currState = State.Hover;
        startHoverTime = DateTime.UtcNow;
        Destroy(collision.gameObject);
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
        transform.GetComponent<Rigidbody2D>().gravityScale /= hoverGravityFactor;
        currState = State.Normal;
    }

    public void KillPlayer()
    {
        currState = State.Dead;
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

public enum State
{
    Normal, Hover, Dead, Gone
}