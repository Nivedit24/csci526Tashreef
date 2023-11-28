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
    public float speedDuplicate = 20f;
    public float jumpSpeedDuplicate = 8f;
    private float direction = 0f;
    public bool faceRight = true;
    public Rigidbody2D playerRB;
    public float groundCheckRadius = 5f;
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
    public DateTime powerEndTime;
    private string transitionLayer = "Transition";
    private string defaultLayer = "Default";
    private bool cloudDrag = false;
    private string beforeTransitionLayer;
    private long sessionID;
    private long deadCounter;
    private long deadSinceLastCheckPoint = 0;
    private int levelName;
    public int energyBallsCounter = 0;
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
    public static bool analytics01Enabled = true;
    public static bool analytics02Enabled = true;

    public int parentPlarformDirection = 0;
    public float parentPlatformSpeed = 0;

    public int fireShotCount = 0;
    public int iceShotCount = 0;
    public int airballTime = 0;
    public int earthShieldTime = 0;
    private int mountStartLevel;
    private int shieldStartLevel;
    public Dictionary<string, int> enemyHits = new Dictionary<string, int>();
    public string lastPowerUsed = "Start";
    public string gameOverSceneName = "GameOverScene";
    public TextMeshProUGUI goldStarsCollectedText;
    public HealthModifier energyBar;
    public TextMeshProUGUI completionText;
    [SerializeField] private List<GameObject> allEnemies;
    [SerializeField] private GameObject clouds;
    [SerializeField] private GameObject barrier;
    [SerializeField] private GameObject allMovingPlatforms;
    [SerializeField] private GameObject allSwitches;
    [SerializeField] private GameObject downWardCanvas;
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
    public bool isFrozen = false;
    private FreezeUnfreezeObject freeze;
    private EnemyFreezeTimer enemyfreezeTimer;
    public Coroutine unFreezeEnemy;
    public EnemyMovement enemyMovement;
    public List<GameObject> heartStore;
    // Start is called before the first frame update
    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        checkPoint = new CheckPoint(transform);
        freeze = GetComponent<FreezeUnfreezeObject>();
        enemyfreezeTimer = GetComponent<EnemyFreezeTimer>();
        currState = State.Normal;
        // For analytics
        deadCounter = 0;
        sessionID = DateTime.Now.Ticks;
        startGameTime = DateTime.Now;
        lastCheckPointTime = DateTime.Now;
        energyBallsCounter = 0;
        speedDuplicate = speed;
        jumpSpeedDuplicate = jumpSpeed;

        energyBar.SetMaxHealth((int)(maxEnergy * 10));

        shootProjectile.enabled = false;
        powerTimer.enabled = false;
        powerEndTime = DateTime.UtcNow;
        for (int i = 0; i < activePowers.Count; i++)
        {
            switch (activePowers[i])
            {
                case Power.Air:
                    elements.transform.GetChild(0).gameObject.SetActive(true);
                    elements.transform.GetChild(4).gameObject.SetActive(true);
                    airPower = true;
                    break;
                case Power.Fire:
                    elements.transform.GetChild(1).gameObject.SetActive(true);
                    elements.transform.GetChild(5).gameObject.SetActive(true);
                    firePower = true;
                    break;
                case Power.Water:
                    elements.transform.GetChild(2).gameObject.SetActive(true);
                    elements.transform.GetChild(6).gameObject.SetActive(true);
                    waterPower = true;
                    break;
                case Power.Earth:
                    elements.transform.GetChild(3).gameObject.SetActive(true);
                    elements.transform.GetChild(7).gameObject.SetActive(true);
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
        isTouchingGround = Physics2D.OverlapCircle(playerRB.position, groundCheckRadius, groundLayer);

        if (currState == State.Shielded)
        {
            isTouchingGround = Physics2D.OverlapCircle(playerRB.position, groundCheckRadius + 3.5f, groundLayer);
        }

        playerRB.velocity = new Vector2(direction * speed, playerRB.velocity.y);

        if (Input.GetKeyDown(KeyCode.UpArrow) && isTouchingGround)
        {
            playerRB.AddForce(new Vector2(playerRB.velocity.x, parentPlarformDirection * parentPlatformSpeed * 5.0f + jumpSpeed), ForceMode2D.Impulse);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow) && !isTouchingGround)
        {
            playerRB.AddForce(new Vector2(playerRB.velocity.x, -jumpSpeed), ForceMode2D.Impulse);
        }
        else if (airPower && Input.GetKeyDown(KeyCode.Z) && energyBallsCounter > 0)
        {
            currPower = Power.Air;
            logoChange(0);
            shootProjectile.enabled = false;
            if (currState == State.Shielded)
            {
                RemoveEarthShield();
            }
            energyLeft = energyBar.slider.value;

            if (currState == State.Hover)
            {
                DismountAirBall();
            }
            else if (energyLeft > 0)
            {
                HoverOnAirBall();
            }
        }
        else if (firePower && Input.GetKeyDown(KeyCode.X) && energyBallsCounter > 0)
        {
            currPower = Power.Fire;
            logoChange(1);
            shootProjectile.enabled = true;
            if (currState == State.Hover)
            {
                DismountAirBall();
            }

            if (currState == State.Shielded)
            {
                RemoveEarthShield();
            }
            currState = State.Normal;
            launchPointDisplay(0);
        }
        else if (waterPower && Input.GetKeyDown(KeyCode.C) && energyBallsCounter > 0)
        {
            currPower = Power.Water;
            logoChange(2);
            shootProjectile.enabled = true;
            if (currState == State.Hover)
            {
                DismountAirBall();
            }
            if (currState == State.Shielded)
            {
                RemoveEarthShield();
            }
            currState = State.Normal;
            launchPointDisplay(1);
        }
        else if (earthPower && Input.GetKeyDown(KeyCode.V) && energyBallsCounter > 0)
        {
            currPower = Power.Earth;
            logoChange(3);
            shootProjectile.enabled = false;
            if (currState == State.Hover)
            {
                DismountAirBall();
            }
            energyLeft = energyBar.slider.value;
            if (currState == State.Shielded)
            {
                RemoveEarthShield();
            }
            else if (energyLeft > 0)
            {
                EquipEarthShield();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space) && energyBallsCounter > 0)
        {
            energyLeft = energyBar.slider.value;
            switch (currPower)
            {
                case Power.Air:
                    if (currState == State.Hover)
                    {
                        DismountAirBall();
                    }
                    else if (energyLeft > 0 && !isFrozen)
                    {
                        HoverOnAirBall();
                    }
                    break;
                case Power.Earth:
                    if (currState == State.Shielded)
                    {
                        RemoveEarthShield();
                    }
                    else if (energyLeft > 0 && !isFrozen)
                    {
                        EquipEarthShield();
                    }
                    break;
                default:
                    break;
            }
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
                else if (currPower == Power.Earth)
                {
                    RemoveEarthShield();
                }
                deadCounter++;
                callDeathCoordinatesAnalytics(playerRB.transform.position);
                deleteHearts();
                ResetUsedMovingPlatforms();
                ResetUsedCollectables(energyBalls);
                ResetAllEnemies();
                RemovePendingIceCubes();
                playerRB.transform.position = checkPoint.position;
                currState = State.Normal;
                break;
            default:
                break;
        }

        if (currPower == Power.Air || currPower == Power.Earth)
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

                if (SceneManager.GetActiveScene().buildIndex <= 6)
                {
                    callCheckPointTimeAnalyticsLevelChange(SceneManager.GetActiveScene().buildIndex - 2); // Each level gets 2 added from now on
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                }
                if (SceneManager.GetActiveScene().buildIndex == 7)
                {
                    callCheckPointTimeAnalyticsLevelChange(SceneManager.GetActiveScene().buildIndex - 2);
                    completionText.gameObject.SetActive(true);
                    speed = 0;
                    jumpSpeed = 0;
                }
                break;
            case "CheckPoint":
                // Checkpoint Analytics Code
                callCheckPointTimeAnalytics(other);

                //Send obstacles analytics 
                foreach (var enemyHit in enemyHits)
                {
                    string obstacleName = enemyHit.Key;
                    long hitCounter = enemyHit.Value;

                    callObstacleCountAnalytics(other, obstacleName, hitCounter);
                }

                // Send Power analytics too
                callPowerUsageAnalytics(other, "Power Airball", airballTime);
                callPowerUsageAnalytics(other, "Power FireShot", fireShotCount);
                callPowerUsageAnalytics(other, "Power IceShot", iceShotCount);
                callPowerUsageAnalytics(other, "Power EarthShield", earthShieldTime);

                goldStarsCollected += 1;
                checkPoint.SetCheckPoint(transform);
                other.gameObject.GetComponent<StarUpdate>().ToggleEnemyStars(false);
                other.gameObject.SetActive(false);
                break;
            case "tempLayerChanger":
                if (transform.position.y > other.transform.position.y)
                {
                    string layer = LayerMask.LayerToName(transform.gameObject.layer);
                    if (layer != transitionLayer)
                    {
                        beforeTransitionLayer = layer;
                    }
                    cloudDrag = true;
                    transform.gameObject.layer = LayerMask.NameToLayer(transitionLayer);
                    if (isHovering)
                    {
                        playerRB.drag = dragFactor;
                    }
                }
                break;
            case "LayerRestorer":
                if (cloudDrag)
                {
                    transform.gameObject.layer = LayerMask.NameToLayer(beforeTransitionLayer);
                    playerRB.drag = 0.0f;
                    cloudDrag = false;
                }
                break;
            case "AcidDrop":
                damageReceiver.TakeDamage(Util.enemyTagToDamage["AcidDrop"], currState == State.Shielded);
                if (currState != State.Shielded)
                {
                    if (enemyHits.ContainsKey("AcidDrop"))
                    {
                        enemyHits["AcidDrop"] += 1;
                    }
                    else
                    {
                        enemyHits.Add("AcidDrop", 1);
                    }
                }
                break;
            case "downWardArrowInstruction":
                if (downWardCanvas)
                {
                    if (transform.position.x < other.transform.position.x)
                    {
                        downWardCanvas.SetActive(false);
                    }
                    else
                    {
                        downWardCanvas.SetActive(true);
                    }
                }
                break;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        switch (other.gameObject.tag)
        {
            case "IceMonster":
                damageReceiver.TakeDamage(Util.enemyTagToDamage["IceMonster"], currState == State.Shielded);
                if (currState != State.Shielded)
                {
                    if (enemyHits.ContainsKey("IceMonster"))
                    {
                        enemyHits["IceMonster"] += 1;
                    }
                    else
                    {
                        enemyHits.Add("IceMonster", 1);
                    }
                }
                break;
            case "WaterBody":
                damageReceiver.TakeDamage(Util.enemyTagToDamage["WaterBody"], currState == State.Shielded);
                if (currState != State.Shielded)
                {
                    if (enemyHits.ContainsKey("WaterBody"))
                    {
                        enemyHits["WaterBody"] += 1;
                    }
                    else
                    {
                        enemyHits.Add("WaterBody", 1);
                    }
                }
                break;
            case "Sand":
                float drag = currState != State.Shielded ? 30f : 0f;
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
                if (obj != null && obj.activeSelf)
                {
                    obj.SetActive(false);
                }
            }
            foreach (GameObject obj in iceCubesOnDoorSwitches)
            {
                if (obj != null && !obj.activeSelf)
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
                powerTimer.enabled = true;

                SetEnergyLevel(maxEnergy);
                if (currState == State.Hover || currState == State.Shielded)
                {
                    powerStartTime = DateTime.UtcNow;
                }
                collision.gameObject.SetActive(false);
                return;
            case "Respawn":
                KillPlayer();
                return;
            case "cloudDirectionChanger":
                Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
                return;
            case "LightningCloud":
                Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
                return;
            case "BreakWall":
                if (currState == State.Shielded)
                {
                    Destroy(collision.gameObject); // Destroy the wall.
                }
                return;
            case "BossSnowball":
                if (!isFrozen && currState != State.Shielded)
                {
                    isFrozen = true;
                    speed = 0;
                    jumpSpeed = 0;
                    Destroy(collision.gameObject);
                    enemyfreezeTimer.enabled = true;
                    enemyfreezeTimer.freezeBar.gameObject.SetActive(true);
                    transform.Find("ice_cube").gameObject.SetActive(true);
                    enemyfreezeTimer.freezeBar.SetMaxHealth((int)5f);
                    enemyfreezeTimer.currHealth = (int)5f;
                    enemyfreezeTimer.InvokeRepeating("reduceFrozenTime", 1.0f, 1.0f);
                    unFreezeEnemy = StartCoroutine(freeze.UnfreezeAfterDelay(5f));
                }
                else if (currState != State.Shielded)
                {
                    enemyfreezeTimer.CancelInvoke();
                    StopCoroutine(unFreezeEnemy);
                    isFrozen = true;
                    Destroy(collision.gameObject);
                    enemyfreezeTimer.enabled = true;
                    enemyfreezeTimer.freezeBar.enabled = true;
                    enemyfreezeTimer.freezeBar.SetMaxHealth((int)5f);
                    enemyfreezeTimer.currHealth = (int)5f;
                    enemyfreezeTimer.InvokeRepeating("reduceFrozenTime", 1.0f, 1.0f);
                    unFreezeEnemy = StartCoroutine(freeze.UnfreezeAfterDelay(5f));
                }
                return;
            case "HeartEnergy":
                damageReceiver.giveHealth();
                collision.gameObject.SetActive(false);
                return;
            default:
                string enemyTag = collision.gameObject.tag;
                Dictionary<string, int> enemyTagToDamage = Util.enemyTagToDamage;
                if (enemyTagToDamage.ContainsKey(enemyTag))
                {
                    int damage = enemyTagToDamage[enemyTag];
                    damageReceiver.TakeDamage(damage, currState == State.Shielded);
                    if (currState != State.Shielded)
                    {
                        if (enemyHits.ContainsKey(enemyTag))
                        {
                            enemyHits[enemyTag] += 1;
                        }
                        else
                        {
                            enemyHits.Add(enemyTag, 1);
                        }
                    }
                }
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
            powerEndTime = DateTime.UtcNow;
        }
    }

    public void updateStarsUI()
    {
        goldStarsCollectedText.text = $"{goldStarsCollected}/{goldStarsRequired}";
        if (goldStarsCollected >= goldStarsRequired)
        {
            barrier.SetActive(false);
        }
    }

    public void HoverOnAirBall()
    {
        mountStartLevel = (int)energyBar.slider.value;
        if (lastPowerUsed != Power.Air.ToString())
        {
            callPowerPairAnalytics(lastPowerUsed, Power.Air.ToString());
        }
        lastPowerUsed = Power.Air.ToString();

        Transform playerBody = transform.Find("Body");
        Transform hiddenHoverball = transform.Find("HoverBall");
        hiddenHoverball.gameObject.SetActive(true);
        hiddenHoverball.GetComponent<RotateAir>().startRotate = true;
        Vector3 bodyPosition = playerBody.localPosition;
        bodyPosition.y += hiddenHoverball.transform.localScale.y;
        playerBody.localPosition = bodyPosition;
        speed *= hoverSpeedFactor;
        jumpSpeed *= hoverJumpFactor;
        playerRB.gravityScale *= hoverGravityFactor;
        playerRB.mass *= hoverMassFactor;
        transform.gameObject.layer = LayerMask.NameToLayer("Cloud");
        currState = State.Hover;
        isHovering = true;
        powerStartTime = DateTime.UtcNow;
        ToggleCloudDirectionArrows(true);
    }

    public void DismountAirBall()
    {
        int temp = mountStartLevel - (int)energyBar.slider.value;
        airballTime += temp;

        Transform hoverBall = transform.Find("HoverBall");
        Transform playerBody = transform.Find("Body");
        Vector3 bodyPosition = playerBody.localPosition;
        transform.gameObject.layer = LayerMask.NameToLayer(defaultLayer);
        bodyPosition.y -= hoverBall.transform.localScale.y;
        hoverBall.gameObject.SetActive(false);
        playerBody.localPosition = bodyPosition;
        speed /= hoverSpeedFactor;
        jumpSpeed /= hoverJumpFactor;
        playerRB.gravityScale /= hoverGravityFactor;
        playerRB.mass /= hoverMassFactor;
        isHovering = false;
        currState = State.Normal;
        energyLeft = energyBar.slider.value;
        powerEndTime = DateTime.UtcNow;
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
        shieldStartLevel = (int)energyBar.slider.value;
        if (lastPowerUsed != Power.Earth.ToString())
        {
            callPowerPairAnalytics(lastPowerUsed, Power.Earth.ToString());
        }
        lastPowerUsed = Power.Earth.ToString();

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
        int temp = shieldStartLevel - (int)energyBar.slider.value;
        earthShieldTime += temp;
        Transform shield = transform.Find("EarthShield");
        shield.gameObject.SetActive(false);
        currState = State.Normal;
        energyLeft = energyBar.slider.value;
        powerEndTime = DateTime.UtcNow;
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
                Transform attachedSwitch = movingPlatform.transform.Find("Switch");
                if (attachedSwitch && attachedSwitch.gameObject.GetComponent<SwitchMovement>().allowResetToParentPlatform)
                {
                    attachedSwitch.gameObject.GetComponent<SwitchMovement>().activated = false;
                }
                i += 1;
            }
        }
    }

    public void KillPlayer()
    {
        currState = State.Dead;
        damageReceiver.giveHealth();
    }

    public void deleteHearts()
    {
        for (int i = 0; i < heartStore.Count; i++)
            Destroy(heartStore[i]);
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

        deadSinceLastCheckPoint = deadCounter - deadSinceLastCheckPoint;

        Analytics02CheckPointTime ob2 = gameObject.AddComponent<Analytics02CheckPointTime>();
        levelName = SceneManager.GetActiveScene().buildIndex - 2; // Each level gets 2 added from now on

        string checkpointName = other.gameObject.name;
        string checkPointNumber = checkpointName.Substring(checkpointName.Length - 2).ToString();

        ob2.Send(sessionID, checkPointNumber.ToString(), levelName.ToString(), checkPointDelta.TotalSeconds, gameTime.TotalSeconds, deadSinceLastCheckPoint);
    }

    public void callObstacleCountAnalytics(Collider2D other, string obstacleName, long hitCounter)
    {
        levelName = SceneManager.GetActiveScene().buildIndex - 2;
        string checkpointName = other.gameObject.name;
        string checkPointNumber = checkpointName.Substring(checkpointName.Length - 2).ToString();

        Analytics03ObstaclesPowers ob3 = gameObject.AddComponent<Analytics03ObstaclesPowers>();

        ob3.Send(sessionID, checkPointNumber, levelName.ToString(), obstacleName, hitCounter);
    }

    public void callPowerUsageAnalytics(Collider2D other, string obstacleName, long hitCounter)
    {
        levelName = SceneManager.GetActiveScene().buildIndex - 2;
        string checkpointName = other.gameObject.name;
        string checkPointNumber = checkpointName.Substring(checkpointName.Length - 2).ToString();

        Analytics03ObstaclesPowers ob3 = gameObject.AddComponent<Analytics03ObstaclesPowers>();
        ob3.Send(sessionID, checkPointNumber, levelName.ToString(), obstacleName, hitCounter);
    }

    public void callPowerPairAnalytics(string power1, string power2)
    {
        levelName = SceneManager.GetActiveScene().buildIndex - 2;

        Analytics03ObstaclesPowers ob3 = gameObject.AddComponent<Analytics03ObstaclesPowers>();
        ob3.Send(sessionID, power1, levelName.ToString(), power2, 1);
    }

    private void callDeathCoordinatesAnalytics(Vector3 position)
    {
        levelName = SceneManager.GetActiveScene().buildIndex - 2;

        Analytics01DeadTime ob1 = gameObject.AddComponent<Analytics01DeadTime>();
        ob1.Send(sessionID, position.x.ToString(), position.y.ToString(), levelName.ToString());
    }

    private void logoChange(int curLogo)
    {
        for (int i = 0; i < 4; i++)
        {
            if (curLogo == i)
            {
                elements.transform.GetChild(i).localScale = new Vector3(1.5f, 1.5f, 1.0f);
            }
            else
                elements.transform.GetChild(i).localScale = new Vector3(0.9f, 0.9f, 1f);
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

    public IEnumerator FreezePlayer()
    {
        speed = 0;
        jumpSpeed = 0;
        transform.Find("ice_cube").gameObject.SetActive(true);
        yield return new WaitForSeconds(5f);
        speed = speedDuplicate;
        jumpSpeed = jumpSpeedDuplicate;
        transform.Find("ice_cube").gameObject.SetActive(false);
    }
}

internal class CheckPoint
{
    public Vector3 position;

    public CheckPoint(Transform transform)
    {
        position = transform.position;
    }

    public void SetCheckPoint(Transform transform)
    {
        position = transform.position;
    }
}

