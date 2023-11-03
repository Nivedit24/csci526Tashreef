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
    public float maxEnergy;
    public float energyLeft;
    public DateTime powerStartTime;
    private string transitionLayer = "Transition";
    private string defaultLayer = "Default";
    private bool cloudDrag = false;
    private string beforeTransitionLayer;
    private long sessionID;
    private long deadCounter;
    private int levelName;
    public int energyBallsCounter;
    private int goldStarsCollected = 0;
    private Rigidbody2D platformRigidbody = null;
    public int goldStarsRequired = 5;
    public List<GameObject> iceCubes = new List<GameObject>();
    public List<GameObject> iceCubesOnDoorSwitches = new List<GameObject>();
    private CheckPoint checkPoint;
    public float dragFactor;
    public State currState;
    public DamageReceiver damageReceiver;
    public bool isHovering = false;
    private DateTime startGameTime, lastCheckPointTime;
    public ShootProjectile shootProjectile;
    public static bool analytics01Enabled = false;
    public static bool analytics02Enabled = true;
    public string gameOverSceneName = "GameOverScene";
    public TextMeshProUGUI goldStarsCollectedText;
    public HealthModifier energyBar;
    public TMP_Text displayText;
    public TextMeshProUGUI completionText;
    [SerializeField] private List<GameObject> instructions;
    [SerializeField] private List<GameObject> allEnemies;
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
    public PowerTimer powerTimer;
    private bool airPower = false;
    private bool firePower = false;
    private bool waterPower = false;
    private bool earthPower = false;
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
        energyBallsCounter = 0;
        energyBar.SetMaxHealth((int)(maxEnergy * 10));

        shootProjectile.enabled = false;
        powerTimer.enabled = false;
        for (int i = 0; i < activePowers.Count; i++)
        {
            switch (activePowers[i])
            {
                case Power.Air:
                    elements.transform.GetChild(0).gameObject.SetActive(true);
                    if (activePowers.Count > 1)
                    {
                        elements.transform.GetChild(8).gameObject.SetActive(true);
                    }
                    airPower = true;
                    break;
                case Power.Fire:
                    elements.transform.GetChild(5).gameObject.SetActive(true);
                    elements.transform.GetChild(9).gameObject.SetActive(true);
                    firePower = true;
                    break;
                case Power.Water:
                    elements.transform.GetChild(6).gameObject.SetActive(true);
                    elements.transform.GetChild(10).gameObject.SetActive(true);
                    waterPower = true;
                    break;
                case Power.Earth:
                    elements.transform.GetChild(7).gameObject.SetActive(true);
                    elements.transform.GetChild(11).gameObject.SetActive(true);
                    earthPower = true;
                    break;
            }
        }

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

        if (currState == State.Shielded)
        {
            isTouchingGround = Physics2D.OverlapCircle(player.position, groundCheckRadius + 3.5f, groundLayer);
        }

        player.velocity = new Vector2(direction * speed, player.velocity.y);

        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) && isTouchingGround)
        {
            player.AddForce(new Vector2(player.velocity.x, jumpSpeed), ForceMode2D.Impulse);
        }
        else if (airPower && Input.GetKeyDown(KeyCode.Z))
        {
            currPower = Power.Air;
            shootProjectile.enabled = false;
            if (currState == State.Shielded)
            {
                RemoveEarthShield();
            }
            if (energyLeft > 0 && currState != State.Hover)
            {
                HoverOnAirBall();
            }
        }
        else if (firePower && Input.GetKeyDown(KeyCode.X))
        {
            currPower = Power.Fire;
            shootProjectile.enabled = true;
            if (currState == State.Hover)
            {
                DismountAirBall();
            }

            if (currState == State.Shielded)
            {
                RemoveEarthShield();
            }
        }
        else if (waterPower && Input.GetKeyDown(KeyCode.C))
        {
            currPower = Power.Water;
            shootProjectile.enabled = true;
            if (currState == State.Hover)
            {
                DismountAirBall();
            }
            if (currState == State.Shielded)
            {
                RemoveEarthShield();
            }
        }
        else if (earthPower && Input.GetKeyDown(KeyCode.V))
        {
            currPower = Power.Earth;
            shootProjectile.enabled = false;
            if (currState == State.Hover)
            {
                DismountAirBall();
            }
            if (energyLeft > 0 && currState != State.Shielded)
            {
                EquipEarthShield();
            }
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
                        if (energyLeft > 0)
                        {
                            HoverOnAirBall();
                        }
                    }
                    break;
                case Power.Fire:
                    break;
                case Power.Water:
                    break;
                case Power.Earth:
                    if (currState == State.Shielded)
                    {
                        RemoveEarthShield();
                    }
                    else
                    {
                        if (energyLeft > 0)
                        {
                            EquipEarthShield();
                        }
                    }


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
        updateStarsUI();
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
                }
                deadCounter++;
                TimeSpan gameTime = DateTime.Now - startGameTime;
                Analytics01DeadTime ob = gameObject.AddComponent<Analytics01DeadTime>();
                levelName = SceneManager.GetActiveScene().buildIndex;
                ob.Send(levelName.ToString(), gameTime.TotalSeconds, deadCounter.ToString(), sessionID);
                ResetUsedMovingPlatforms();
                ResetUsedCollectables(energyBalls);
                ResetAllEnemies();
                RemovePendingIceCubes();
                player.transform.position = checkPoint.position;
                currState = State.Normal;
                return;
            case State.Normal:
                powerTimer.enabled = false;
                break;
            case State.Hover:
                powerTimer.enabled = true;
                break;

            case State.Shielded:
                powerTimer.enabled = true;

                break;

            default:
                return;
        }

        if (currPower == Power.Air || currPower == Power.Earth || energyLeft <= 0)
        {
            removeLaunchPointDisplays();
        }
        else
        {
            int idx = currPower == Power.Fire ? 0 : 1;
            removeLaunchPointDisplays();
            launchPointDisplay(idx);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (other.gameObject.tag)
        {
            case "Goal":
                Debug.Log("Fire Log Triggered");
                if (SceneManager.GetActiveScene().buildIndex <= 5)
                {
                    callCheckPointTimeAnalyticsLevelChange(SceneManager.GetActiveScene().buildIndex - 2); // Each level gets 2 added from now on
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                }
                if (SceneManager.GetActiveScene().buildIndex == 6)
                {
                    completionText.gameObject.SetActive(true);
                    speed = 0;
                    jumpSpeed = 0;
                }
                break;
            case "CheckPoint":

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
            case "AcidDrop":
                Debug.Log("Collided with Acid drop");
                damageReceiver.TakeDamage(5, currState == State.Shielded);
                break;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        switch (other.gameObject.tag)
        {
            case "IceMonster":
                damageReceiver.TakeDamage(10, currState == State.Shielded);
                break;
            case "WaterBody":
                break;
            case "Sand":
                float drag = currState != State.Shielded ? 50f : 0f;
                drag = currState == State.Hover ? 10f : drag;
                transform.GetComponent<Rigidbody2D>().drag = drag;
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        switch (other.gameObject.tag)
        {
            case "Sand":
                transform.GetComponent<Rigidbody2D>().drag = 0f;
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

    void RemovePendingIceCubes()
    {
        if (iceCubes != null)
        {
            foreach (GameObject obj in iceCubes)
            {
                if (obj.activeSelf)
                {
                    obj.SetActive(false);
                }
            }
            foreach (GameObject obj in iceCubesOnDoorSwitches)
            {
                if (!obj.activeSelf)
                {
                    obj.SetActive(true);
                }
            }
            iceCubes.Clear();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        switch (collision.gameObject.tag)
        {
            case "EnergyBall":
                // Analytics for energy ball
                energyBallsCounter++;
                callEnergyBallCounterAnalytics(energyBallsCounter);

                Debug.Log("Collision with energy ball");
                if (instructions.Contains(collision.gameObject))
                {
                    DisplayText("Replenish your Energy", collision.gameObject);
                }
                SetEnergyLevel(maxEnergy);
                if (currState == State.Hover || currState == State.Shielded)
                {
                    powerStartTime = DateTime.UtcNow;
                }
                collision.gameObject.SetActive(false);
                break;
            case "Respawn":
                KillPlayer();
                break;
            case "Tornado":
                Debug.Log("Player is hit by Tornado");
                damageReceiver.TakeDamage(10, currState == State.Shielded);
                break;
            case "lightning":
                Debug.Log("Struck by Lightning");
                damageReceiver.TakeDamage(25, currState == State.Shielded);
                break;
            case "cloudDirectionChanger":
                Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
                break;
            case "LightningCloud":
                Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
                break;
            case "Demon":
                Debug.Log("Hit by demon");
                damageReceiver.TakeDamage(20, currState == State.Shielded);
                break;
            case "VolcanoBall":
                Debug.Log("Hit by volcanoBall");
                damageReceiver.TakeDamage(50, currState == State.Shielded);
                break;
            case "DemonFireball":
                Debug.Log("Hit by DemonFireBall");
                damageReceiver.TakeDamage(25, currState == State.Shielded);
                break;
            case "DeathFloor":
                Debug.Log("Player is hit by Death Floor");
                damageReceiver.TakeDamage(25, currState == State.Shielded);
                break;
            case "EarthMonster":
                damageReceiver.TakeDamage(25, currState == State.Shielded);
                break;
            case "BreakWall":
                if (currState == State.Shielded)
                {
                    Destroy(collision.gameObject); // Destroy the wall.
                }
                break;
            default:
                break;
        }
    }

    public void SetEnergyLevel(float energy)
    {
        energyBar.gameObject.SetActive(true);
        energyBar.SetHealth((int)(energy * 10));
        energyLeft = energy * 10;

        if (energy == 0)
        {
            energyBar.gameObject.SetActive(false);
        }
        else
        {
            energyBar.gameObject.SetActive(true);
        }
    }

    private void DisplayText(string message, GameObject obj)
    {
        displayText.text = message;
        Invoke("HideTextAfterDelay", 3f);
        instructions.Remove(obj);
    }

    public void updateStarsUI()
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

    public void HoverOnAirBall()
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
        powerStartTime = DateTime.UtcNow;
        ToggleCloudDirectionArrows(true);
    }

    public void DismountAirBall()
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
        energyLeft = energyBar.slider.value;
        ToggleCloudDirectionArrows(false);
    }

    private void ToggleCloudDirectionArrows(bool show)
    {
        if (clouds == null)
        {
            return;
        }
        foreach (Transform cloud in clouds.transform)
        {
            Transform child = cloud.GetChild(0);
            child.gameObject.SetActive(show);
        }
    }
    void EquipEarthShield()
    {
        Transform shield = transform.Find("EarthShield");
        shield.gameObject.SetActive(true);
        Transform playerBody = transform.Find("Body");
        Vector3 bodyPosition = playerBody.localPosition;
        bodyPosition.y += shield.transform.localScale.y;
        currState = State.Shielded;
        shield.GetComponent<RotateShield>().startRotate = true;
        powerStartTime = DateTime.UtcNow;
    }
    public void RemoveEarthShield()
    {
        Transform shield = transform.Find("EarthShield");
        shield.gameObject.SetActive(false);
        currState = State.Normal;
        energyLeft = energyBar.slider.value;
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

    public void ResetAllEnemies()
    {
        if (allEnemies != null)
        {
            for (int i = 0; i < allEnemies.Count; i++)
            {
                foreach (Transform enemy in allEnemies[i].transform)
                {
                    enemy.gameObject.GetComponentInChildren<HealthModifier>().SetMaxHealth(enemy.gameObject.GetComponent<EnemyDamage>().maxHealth);
                    enemy.gameObject.GetComponent<EnemyDamage>().currHealth = enemy.gameObject.GetComponent<EnemyDamage>().maxHealth;
                    enemy.gameObject.SetActive(true);
                }
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
        levelName = SceneManager.GetActiveScene().buildIndex - 2; // Each level gets 2 added from now on

        string checkpointName = other.gameObject.name;
        string checkPointNumber = checkpointName.Substring(checkpointName.Length - 2).ToString();
        print("CheckPointName: " + checkPointNumber);
        ob2.Send(sessionID, checkPointNumber.ToString(), levelName.ToString(), checkPointDelta.TotalSeconds, gameTime.TotalSeconds, deadCounter);
    }

    public void callEnergyBallCounterAnalytics(int energyBallsCounter)
    {
        TimeSpan gameTime = DateTime.Now - startGameTime;
        TimeSpan checkPointDelta = DateTime.Now - lastCheckPointTime;
        lastCheckPointTime = DateTime.Now;// EnergyBallsCounter is calculated in time from the last checkPoint

        Analytics02CheckPointTime ob2 = gameObject.AddComponent<Analytics02CheckPointTime>();
        levelName = SceneManager.GetActiveScene().buildIndex - 2; // Each level gets 2 added from now on
        // print("EnergyBallC Counter: " + energyBallsCounter);

        ob2.Send(sessionID, "Energy Ball", levelName.ToString(), (double)energyBallsCounter, gameTime.TotalSeconds, deadCounter);
    }

    private void logoChange(int curLogo)
    {
        for (int i = 0; i < 4; i++)
        {
            if (curLogo - 4 == i)
            {
                if (elements.transform.GetChild(curLogo).gameObject.activeSelf)
                {
                    elements.transform.GetChild(curLogo - 4).gameObject.SetActive(true);
                    elements.transform.GetChild(curLogo).gameObject.SetActive(false);
                }
            }
            else
            {
                if (elements.transform.GetChild(i).gameObject.activeSelf)
                {
                    elements.transform.GetChild(i).gameObject.SetActive(false);
                    elements.transform.GetChild(i + 4).gameObject.SetActive(true);
                }
            }
        }
    }

    private void launchPointDisplay(int childOne)
    {
        if (faceRight)
        {
            transform.GetChild(2).GetChild(childOne).gameObject.SetActive(true);
            transform.GetChild(3).GetChild(childOne).gameObject.SetActive(false);
        }
        else
        {
            transform.GetChild(2).GetChild(childOne).gameObject.SetActive(false);
            transform.GetChild(3).GetChild(childOne).gameObject.SetActive(true);
        }
    }

    private void removeLaunchPointDisplays()
    {
        transform.GetChild(2).GetChild(0).gameObject.SetActive(false);
        transform.GetChild(2).GetChild(1).gameObject.SetActive(false);
        transform.GetChild(3).GetChild(0).gameObject.SetActive(false);
        transform.GetChild(3).GetChild(1).gameObject.SetActive(false);
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
    Normal, Hover, Shielded, Dead, Gone
}

public enum Power
{
    Air, Fire, Water, Earth
}