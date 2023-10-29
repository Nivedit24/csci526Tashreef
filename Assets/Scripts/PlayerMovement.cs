using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.UI;
using System.Threading;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 20f;
    public float jumpSpeed = 8f;
    private float direction = 0f;
    public bool faceRight = true;
    private Rigidbody2D player;
    public float groundCheckRadius = 2.5f;
    public LayerMask groundLayer;
    private bool isTouchingGround;
    private bool isInsideCloud;
    public float hoverSpeedFactor = 2f;
    public float hoverGravityFactor = 0.75f;
    public float hoverJumpFactor = 0.5f;
    public float hoverMassFactor = 0.2f;
    public float hoverTime;
    private DateTime startHoverTime;
    private string transitionLayer = "Transition";
    private string defaultLayer = "Default";
    private bool cloudDrag = false;
    private string beforeTransitionLayer;
    private long sessionID;
    private long deadCounter;
    private int levelName;
    private int goldStarsCollected = 0;
    private Rigidbody2D platformRigidbody = null;
    public int goldStarsRequired = 5;

    private CheckPoint checkPoint;
    public float dragFactor;
    public static State currState;
    public DamageReceiver damageReceiver;
    public bool isHovering = false;
    private DateTime startGameTime, lastCheckPointTime;
    public FireProjectile fireProjectile;
    public static bool analytics01Enabled = false;
    public static bool analytics02Enabled = true;

    public string gameOverSceneName = "GameOverScene";
    public TextMeshProUGUI goldStarsCollectedText;

    public HealthModifier hoverFuel;
    public TMP_Text displayText;
    [SerializeField] private List<GameObject> instructions;
    [SerializeField] private GameObject allDemons;
    [SerializeField] private GameObject clouds;
    [SerializeField] private GameObject barrier;
    [SerializeField] private GameObject allMovingPlatforms;
    [SerializeField] private GameObject allSwitches;
    [SerializeField] public List<Power> activePowers;
    private List<Vector3> initialPositionsOfMovingPlatforms = new List<Vector3>();
    private List<int> initialSwitchDirection = new List<int>();
    private List<bool> initialSwitchActivation = new List<bool>();
    public GameObject energyBalls;
    public Power currPower = Power.Air;
    public GameObject elements;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Rigidbody2D>();
        checkPoint = new CheckPoint(transform);
        currState = State.Normal;
        // For analytics
        deadCounter = 0;
        sessionID = DateTime.Now.Ticks;
        startGameTime = DateTime.Now;
        lastCheckPointTime = DateTime.Now;
        for (int i = 0; i < activePowers.Count; i++)
        {
            switch (activePowers[i])
            {
                case Power.Air:
                    elements.transform.GetChild(0).gameObject.SetActive(true);
                    elements.transform.GetChild(8).gameObject.SetActive(true);
                    break;
                case Power.Fire:
                    elements.transform.GetChild(5).gameObject.SetActive(true);
                    elements.transform.GetChild(9).gameObject.SetActive(true);
                    break;
                case Power.Water:
                    elements.transform.GetChild(6).gameObject.SetActive(true);
                    elements.transform.GetChild(10).gameObject.SetActive(true);
                    break;
                case Power.Earth:
                    elements.transform.GetChild(7).gameObject.SetActive(true);
                    elements.transform.GetChild(11).gameObject.SetActive(true);
                    break;
            }

        }
        
        fireProjectile.enabled = false;
        if (allMovingPlatforms != null)
        {
            foreach (Transform movingPlatform in allMovingPlatforms.transform)
            {
                initialPositionsOfMovingPlatforms.Add(movingPlatform.position);
            }

            foreach (Transform Switch in allSwitches.transform)
            {
                initialSwitchDirection.Add(Switch.GetComponent<SwitchMovement>().direction);
                initialSwitchActivation.Add(Switch.GetComponent<SwitchMovement>().activated);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        direction = Input.GetAxis("Horizontal");
        isTouchingGround = Physics2D.OverlapCircle(player.position, groundCheckRadius, groundLayer);

        player.velocity = new Vector2(direction * speed, player.velocity.y);

        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && isTouchingGround)
        {
            player.AddForce(new Vector2(player.velocity.x, jumpSpeed), ForceMode2D.Impulse);
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            currPower = Power.Air;
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            currPower = Power.Fire;
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            currPower = Power.Water;
        }
        else if (Input.GetKeyDown(KeyCode.Y))
        {
            currPower = Power.Earth;
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            switch (currPower)
            {
                case Power.Air:
                    if (currState == State.Hover)
                    {
                        DismountAirBall();
                    }
                    else
                    {
                        HoverOnAirBall();

                    }
                    break;
                case Power.Fire:
                    fireProjectile.enabled = !fireProjectile.enabled;
                    break;
                case Power.Water:
                    break;
                case Power.Earth:
                    break;
                default:
                    break;
            }
        }

        if (Input.GetKeyDown(KeyCode.Z) && activePowers.Contains(Power.Air))
        {
            logoChange(4);
        }
        if (Input.GetKeyDown(KeyCode.X) && activePowers.Contains(Power.Fire))
        {
            logoChange(5);
        }
        if (Input.GetKeyDown(KeyCode.C) && activePowers.Contains(Power.Water))
        {
            logoChange(6);
        }
        if (Input.GetKeyDown(KeyCode.V) && activePowers.Contains(Power.Earth))
        {
            logoChange(7);
        }
        updateUI();
        if (direction > 0)
        {
            faceRight = true;
        }
        else if (direction < 0)
        {
            faceRight = false;
        }

        switch (currState)
        {
            case State.Dead:
                if (isHovering)
                {
                    DismountAirBall();
                    hoverFuel.gameObject.SetActive(false);
                }
                deadCounter++;
                TimeSpan gameTime = DateTime.Now - startGameTime;
                Analytics01DeadTime ob = gameObject.AddComponent<Analytics01DeadTime>();
                levelName = SceneManager.GetActiveScene().buildIndex;
                ob.Send(levelName.ToString(), gameTime.TotalSeconds, deadCounter.ToString(), sessionID);
                ResetUsedMovingPlatforms();
                ResetAllDemons();
                player.transform.position = checkPoint.position;
                currState = State.Normal;

                return;
            case State.Normal:
                break;
            case State.Hover:
                TimeSpan span = DateTime.UtcNow - startHoverTime;
                hoverFuel.SetHealth((int)((hoverTime - span.TotalSeconds) * 10));
                if (span.TotalSeconds > hoverTime)
                {
                    DismountAirBall();
                    currState = State.Normal;
                    hoverFuel.gameObject.SetActive(false);
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

                //Add Checkpoint Analytics Code
                callCheckPointTimeAnalytics(other);
                goldStarsCollected += 1;
                checkPoint.SetCheckPoint(transform);
                other.gameObject.SetActive(false);
                if (instructions.Contains(other.gameObject))
                {
                    DisplayText("Collect stars to checkpoint your progress", other.gameObject);
                }
                break;
            case "tempLayerChanger":
                if (transform.position.y > other.transform.position.y)
                {
                    Debug.Log("temporary layer change");
                    string layer = LayerMask.LayerToName(transform.gameObject.layer);
                    if (layer != transitionLayer)
                    {
                        beforeTransitionLayer = layer;
                    }
                    cloudDrag = true;
                    transform.gameObject.layer = LayerMask.NameToLayer(transitionLayer);
                    if (isHovering)
                    {
                        player.drag = dragFactor;
                    }
                }
                break;
            case "LayerRestorer":
                if (cloudDrag)
                {
                    transform.gameObject.layer = LayerMask.NameToLayer(beforeTransitionLayer);
                    Debug.Log("beforeTransitionLayer : " + beforeTransitionLayer);
                    player.drag = 0.0f;
                    cloudDrag = false;
                }
                break;
            case "PlatformHolder":
                transform.SetParent(other.transform);
                Debug.Log("moving platform");
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        switch (other.gameObject.tag)
        {
            case "PlatformHolder":
                transform.SetParent(null);
                break;
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        // Check if the player is colliding with a platform
        if (collision.gameObject.CompareTag("Ground"))
        {
            platformRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
        }
    }

    void OnCollisionExit(Collision collision)
    {
        // Reset the platform reference when the player leaves the platform
        if (collision.gameObject.CompareTag("Ground"))
        {
            platformRigidbody = null;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        switch (collision.gameObject.tag)
        {
            case "Goal":
                if (SceneManager.GetActiveScene().buildIndex <= 3)
                {
                    callCheckPointTimeAnalyticsLevelChange(SceneManager.GetActiveScene().buildIndex);
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                }
                break;
            case "EnergyBall":
                Debug.Log("Collision with energy ball");
                if (instructions.Contains(collision.gameObject))
                {
                    DisplayText("Replenish your Energy", collision.gameObject);
                }

                collision.gameObject.SetActive(false);
                break;
            case "Respawn":
                KillPlayer();
                break;
            case "Tornado":
                Debug.Log("Player is hit by Tornado");
                damageReceiver.TakeDamage(10);
                break;
            case "lightning":
                Debug.Log("Struck by Lightning");
                damageReceiver.TakeDamage(25);
                break;
            case "cloudDirectionChanger":
                Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
                break;
            case "LightningCloud":
                Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
                break;
            case "Demon":
                Debug.Log("Hit by demon");
                damageReceiver.TakeDamage(20);
                break;
            case "Fireball":
                if (!fireProjectile.enabled)
                {
                    fireProjectile.enabled = true;
                    collision.gameObject.SetActive(false);
                }
                else
                {
                    fireProjectile.collectFireballs();
                    collision.gameObject.SetActive(false);
                }
                if (instructions.Contains(collision.gameObject))
                {
                    DisplayText("Use Space to shoot", collision.gameObject);
                }
                break;
            case "VolcanoBall":
                Debug.Log("Hit by volcanoBall");
                damageReceiver.TakeDamage(50);
                break;
            case "DemonFireball":
                Debug.Log("Hit by DemonFireBall");
                damageReceiver.TakeDamage(30);
                break;
            case "DeathFloor":
                Debug.Log("Player is hit by Death Floor");
                damageReceiver.TakeDamage(30);
                break;
            
            default:
                break;
        }
    }

    private void DisplayText(string message, GameObject obj)
    {
        displayText.text = message;
        Invoke("HideTextAfterDelay", 3f);
        instructions.Remove(obj);
    }

    public void updateUI()
    {
        goldStarsCollectedText.text = $"{goldStarsCollected}/{goldStarsRequired}";
        if (goldStarsCollected >= goldStarsRequired)
        {
            barrier.SetActive(false);
        }
    }

    void HideTextAfterDelay()
    {
        displayText.text = "";
    }

    void HoverOnAirBall()
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
        transform.gameObject.layer = LayerMask.NameToLayer("Cloud");
        currState = State.Hover;
        isHovering = true;
        hoverFuel.gameObject.SetActive(true);
        hoverFuel.SetMaxHealth((int)(hoverTime * 10));
        startHoverTime = DateTime.UtcNow;

        ToggleCloudDirectionArrows(true);
    }

    void DismountAirBall()
    {
        Transform hoverBall = transform.Find("HoverBall");
        Transform playerBody = transform.Find("Body");
        Vector3 bodyPosition = playerBody.localPosition;
        transform.gameObject.layer = LayerMask.NameToLayer(defaultLayer);
        bodyPosition.y -= hoverBall.transform.localScale.y;
        hoverBall.gameObject.SetActive(false);
        playerBody.localPosition = bodyPosition;
        speed /= hoverSpeedFactor;
        jumpSpeed /= hoverJumpFactor;
        transform.GetComponent<Rigidbody2D>().gravityScale /= hoverGravityFactor;
        transform.GetComponent<Rigidbody2D>().mass /= hoverMassFactor;
        isHovering = false;
        currState = State.Normal;
        ResetUsedCollectables(energyBalls);
        ToggleCloudDirectionArrows(false);
    }

    private void ToggleCloudDirectionArrows(bool show)
    {
        foreach (Transform cloud in clouds.transform)
        {
            Transform child = cloud.GetChild(0);
            child.gameObject.SetActive(show);
        }
    }

    public void ResetUsedCollectables(GameObject collectables)
    {
        if (collectables == null)
        {
            return;
        }
        foreach (Transform collectable in collectables.transform)
        {
            collectable.gameObject.SetActive(true);
        }
    }

    public void ResetAllDemons()
    {
        if (allDemons != null)
        {
            foreach (Transform demon in allDemons.transform)
            {
                demon.gameObject.SetActive(true);
            }
        }
    }



    public void ResetUsedMovingPlatforms()
    {
        if (allSwitches != null)
        {
            int i = 0;
            foreach (Transform Switch in allSwitches.transform)
            {
                Switch.GetComponent<SwitchMovement>().activated = initialSwitchActivation[i];
                i += 1;
            }
            i = 0;
            foreach (Transform Switch in allSwitches.transform)
            {
                Switch.GetComponent<SwitchMovement>().direction = initialSwitchDirection[i];
                i += 1;
            }
        }
        if (allMovingPlatforms != null)
        {
            int i = 0;
            foreach (Transform movingPlatform in allMovingPlatforms.transform)
            {
                movingPlatform.transform.position = initialPositionsOfMovingPlatforms[i];
                i += 1;
            }
        }

    }

    public void KillPlayer()
    {
        currState = State.Dead;
        damageReceiver.currHealth = damageReceiver.maxHealth;
    }

    public void callCheckPointTimeAnalyticsLevelChange(int levelName)
    {
        TimeSpan gameTime = DateTime.Now - startGameTime;


        TimeSpan checkPointDelta = DateTime.Now - lastCheckPointTime;
        lastCheckPointTime = DateTime.Now;

        Analytics02CheckPointTime ob2 = gameObject.AddComponent<Analytics02CheckPointTime>();

        ob2.Send(sessionID, "Level Crossed", levelName.ToString(), checkPointDelta.TotalSeconds, gameTime.TotalSeconds, deadCounter);
    }

    public void callCheckPointTimeAnalytics(Collider2D other)
    {
        TimeSpan gameTime = DateTime.Now - startGameTime;
        TimeSpan checkPointDelta = DateTime.Now - lastCheckPointTime;
        lastCheckPointTime = DateTime.Now;

        Analytics02CheckPointTime ob2 = gameObject.AddComponent<Analytics02CheckPointTime>();
        levelName = SceneManager.GetActiveScene().buildIndex;

        string checkpointName = other.gameObject.name;
        string checkPointNumber = checkpointName.Substring(checkpointName.Length - 2).ToString();
        ob2.Send(sessionID, checkPointNumber.ToString(), levelName.ToString(), checkPointDelta.TotalSeconds, gameTime.TotalSeconds, deadCounter);
    }

    private void logoChange(int curLogo)
    {
        for(int i = 0; i < 4; i++)
        {
            if(curLogo - 4 == i)
            {
                if (elements.transform.GetChild(curLogo).gameObject.activeSelf)
                {
                    elements.transform.GetChild(curLogo-4).gameObject.SetActive(true);
                    elements.transform.GetChild(curLogo).gameObject.SetActive(false);
                }
            }
            else
            {
                if (elements.transform.GetChild(i).gameObject.activeSelf)
                {
                    elements.transform.GetChild(i).gameObject.SetActive(false);
                    elements.transform.GetChild(i+4).gameObject.SetActive(true);
                }
            }
        }
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

public enum Power
{
    None, Air, Fire, Water, Earth
}