using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 20f;
    public float jumpSpeed = 8f;
    private float direction = 0f;
    public bool canMove = true;
    private Rigidbody2D player;

    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;
    private bool isTouchingGround;
    private CheckPoint _checkPoint;
    private State _curState;


    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Rigidbody2D>();
        _checkPoint = new CheckPoint(transform);
        //Debug.Log("position of checkpoint : "+_checkPoint.Position);
        _curState = State.Normal;
    }

    // Update is called once per frame
    void Update()
    {
        var trans = player.transform;
        isTouchingGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        direction = Input.GetAxis("Horizontal");

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

        switch (_curState)
        {
            case State.Dead:
                trans.position = _checkPoint.Position;
                _curState = State.Normal;
                return;
            case State.Normal:
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
                Debug.Log("Checkpoint is " + _checkPoint.Position);
                _checkPoint.DoCheckPoint(transform);
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
                _curState = State.Dead;
                break;
        }
    }
}

internal class CheckPoint
{
    public Vector3 Position;

    public CheckPoint(Transform transform)
    {
        Position = transform.position;
        Debug.LogFormat("Initial CheckPoint Position: ", Position);
    }

    public void DoCheckPoint(Transform transform)
    {
        Position = transform.position;
        Debug.LogFormat("position: ", Position);
    }
}

internal enum State
{
    Normal, Dead, Gone
}