using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;
using System.Threading;


public class PlayerMovement : MonoBehaviour
{
    public float speed = 20f;
    public float jumpSpeed = 8f;
    private float direction = 0f;

    public bool canMove = true;
    private Rigidbody2D player;
    public float groundCheckRadius = 5.0f;
    public LayerMask groundLayer;
    public float cloudCheckRadius = 5.0f;
    public LayerMask cloudLayer;
    private bool isTouchingGround;
    private bool isInsideCloud;
    public float hoverSpeedFactor = 2f;
    public float hoverGravityFactor = 0.75f;
    public float hoverJumpFactor = 0.5f;
    public float hoverMassFactor = 0.2f;
    public float hoverTime;
    private DateTime startHoverTime;
    private long sessionID;
    private long deadCounter;

    private CheckPoint checkPoint;
    public static State currState;
    public DamageReceiver playerReceiver;
    public bool isHovering = false;

    private DateTime startGameTime;

    //public static bool analytics01Enabled = false;
    public static bool analytics01Enabled = true;

    //public static bool analytics02Enabled = false;
    public static bool analytics02Enabled = true;

    public string gameOverSceneName = "GameOverScene";

    [SerializeField] private GameObject allCollectables;
    [SerializeField] private List<GameObject> collectables;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Rigidbody2D>();
        checkPoint = new CheckPoint(transform);
        currState = State.Normal;
        
        deadCounter = 0;
        sessionID = DateTime.Now.Ticks;
        startGameTime = DateTime.Now;

        foreach (Transform childTransf in allCollectables.transform)
        {
            String tag = childTransf.gameObject.tag;
            if (tag == "Airball")
            {
                collectables.Add(childTransf.gameObject);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        isTouchingGround = Physics2D.OverlapCircle(player.position, groundCheckRadius, groundLayer);
        isInsideCloud = Physics2D.OverlapCircle(player.position, cloudCheckRadius, cloudLayer);

        direction = Input.GetAxis("Horizontal");

        if (canMove)
        {
            player.velocity = new Vector2(direction * speed, player.velocity.y);

            if (Input.GetButtonDown("Jump") && (isTouchingGround || (isHovering && isInsideCloud)))
            {
                player.AddForce(new Vector2(player.velocity.x, jumpSpeed), ForceMode2D.Impulse);
            }
        }

        switch (currState)
        {
            case State.Dead:
                if (isHovering)
                {
                    DismountAirBall();
                }

                //Debug.Log("Player entered dead state");
                deadCounter++;
                TimeSpan gameTime = DateTime.Now - startGameTime;
                
                Analytics01DeadTime ob = gameObject.AddComponent<Analytics01DeadTime>();
                ob.Send("Level1", gameTime.TotalSeconds, deadCounter.ToString(), sessionID);

                player.transform.position = checkPoint.position;
                ResetAllCollectables();
                currState = State.Normal;

                return;
            case State.Normal:
                break;
            case State.Hover:
                TimeSpan span = DateTime.UtcNow - startHoverTime;
                if (span.TotalSeconds > hoverTime)
                {
                    DismountAirBall();
                    currState = State.Normal;
                }
                break;
            default:
                return;
        }

    }

    private void ResetAllCollectables()
    {
        foreach (var obj in collectables)
        {
            obj.SetActive(true);
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
                    collision.gameObject.SetActive(false);
                    startHoverTime = DateTime.UtcNow;
                }
                break;
            case "Respawn":
                currState = State.Dead;
                break;
            case "Tornado":
                Debug.Log("Player is hit by Tornado");
                playerReceiver.TakeDamage(10);
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
                playerReceiver.TakeDamage(30);
                break;
            default:
                break;
        }
    }
    void HoverOnAirBall(Collision2D collision)
    {
        Transform playerBody = transform.Find("Body");
        Transform hiddenHoverball = transform.Find("HoverBall");
        hiddenHoverball.gameObject.SetActive(true);
        hiddenHoverball.GetComponent<RotateAir>().startRotate = true;
        Vector3 bodyPosition = playerBody.localPosition;
        bodyPosition.y += hiddenHoverball.transform.localScale.y;
        playerBody.localPosition = bodyPosition;
        speed *= hoverSpeedFactor;
        jumpSpeed *= hoverJumpFactor;
        transform.GetComponent<Rigidbody2D>().gravityScale *= hoverGravityFactor;
        transform.GetComponent<Rigidbody2D>().mass *= hoverMassFactor;
        transform.gameObject.layer = LayerMask.NameToLayer("HoverballLayer");
        currState = State.Hover;
        isHovering = true;
        startHoverTime = DateTime.UtcNow;
        collision.gameObject.SetActive(false);
    }

    void DismountAirBall()
    {
        Transform hoverBall = transform.Find("HoverBall");
        Transform playerBody = transform.Find("Body");
        Vector3 bodyPosition = playerBody.localPosition;
        transform.gameObject.layer = LayerMask.NameToLayer("Default");
        bodyPosition.y -= hoverBall.transform.localScale.y;
        hoverBall.gameObject.SetActive(false);
        playerBody.localPosition = bodyPosition;
        speed /= hoverSpeedFactor;
        jumpSpeed /= hoverJumpFactor;
        transform.GetComponent<Rigidbody2D>().gravityScale /= hoverGravityFactor;
        transform.GetComponent<Rigidbody2D>().mass /= hoverMassFactor;
        isHovering = false;
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