using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    //Player variables
    [Header("Player")]
    [SerializeField] private GameObject player;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private LayerMask rayCastLayer;
    [SerializeField] private Camera cam;
    [SerializeField] private InputActionAsset playerControls;
    //Movement Variables
    [Header("Horizontal Movement")]
    
    [Tooltip("The maximum horizontal speed of the player")]
    [SerializeField] private float speed;
    
    [Tooltip("How many times faster the player is while sprinting (based off speed)")]
    [SerializeField] private float sprintMultiplier;
    
    [Tooltip("How fast the player speeds up horizontally")]
    [SerializeField] private float acceleration;

    [Tooltip("How fast the player slows down horizontally on the ground")]
    [SerializeField] private float groundDeceleration;

    [Tooltip("How fast the player slows down horizontally in the air")]
    [SerializeField] private float airDeceleration;

    [Header("Jump Movement")]

    [Tooltip("How high the player jumps")]
    [SerializeField] private float jumpPower;

    [Tooltip("How fast the player falls")]
    [SerializeField] private float gravity;

    [Tooltip("Multiplies gravity if jump was ended early to achieve a lower jump height")]
    [SerializeField] private float endedJumpEarlyGravityModifier;

    [Tooltip("Multiplies gravity when falling")]
    [SerializeField] private float fallGravityModifier;

    [Tooltip("The maximum speed the player can fall (Terminal Velocity)")]
    [SerializeField] private float maxFallSpeed;

    [Tooltip("The time the player is still able to jump after leaving the ground")]
    [SerializeField] private float coyoteJumptime;

    [Tooltip("The time the player can input a jump before being able to (and then jumping immediately when able)")]
    [SerializeField] private float bufferJumpTime;

    [Tooltip("The force applied to keep the player on the ground")]
    [SerializeField] private float groundingForce;

    [Header("Attack Variables")]

    [Tooltip("The cooldown for the primary attack in stage 1")]
    [SerializeField] private float primaryAttackStage1Cooldown;

    [Tooltip("The cooldown for the primary attack in stage 2")]
    [SerializeField] private float primaryAttackStage2Cooldown;

    [Tooltip("The cooldown for the secondary attack in stage 1")]
    [SerializeField] private float secondaryAttackStage1Cooldown;

    [Tooltip("The cooldown for the secondary attack in stage 2")]
    [SerializeField] private float secondaryAttackStage2Cooldown;

    [Tooltip("The radius the enlarging circle starts at")]
    [SerializeField] private float secondaryAttackStage2StartSize;

    [Tooltip("The max radius the circle can be")]
    [SerializeField] private float secondaryAttackStage1Size;

    [Tooltip("The max radius the enlarging circle can be")]
    [SerializeField] private float secondaryAttackStage2MaxSize;

    [Tooltip("Rate of how fast the circle expands")]
    [SerializeField] private float growthModifierSecondaryAttackStage2;

    [SerializeField] private Transform rightLaunchOffset;
    [SerializeField] private Transform leftLaunchOffset;

    [SerializeField] private Transform rightAttackTransform;
    [SerializeField] private Transform leftAttackTransform;
    [SerializeField] private float stage1AttackRange = 0.5f;
    [SerializeField] private float stage2AttackRange = 1.5f;
    [SerializeField] private LayerMask attackLayer;

    [Header("Mana")]

    [Tooltip("The max amount of mana the player can have")]
    [SerializeField] private int maxMana;

    [Tooltip("The amount of mana that regenerates per second")]
    [SerializeField] private int manaRegeneration;

    [Tooltip("The cost of the primary attack stage 1")]
    [SerializeField] private int primaryAttackStage1Cost;

    [Tooltip("The cost of the primary attack stage 1")]
    [SerializeField] private int primaryAttackStage2Cost;

    [Tooltip("The cost of the primary attack stage 1")]
    [SerializeField] private int secondaryAttackStage1Cost;

    [Tooltip("The cost of the primary attack stage 1")]
    [SerializeField] private int secondaryAttackStage2Cost;

    private int currentMana = 0;

    GameManager gameManager;
    private InputAction sprintAction;
    private InputAction jumpAction;
    private InputAction primaryAttackAction;
    private InputAction secondaryAttackAction;
    private InputAction stageChangeAction;

    private WeaponStage weaponStage;
    private bool doStage2SecondaryAttack;

    private float horizontalMove;
    private Vector2 currentMovement;

    private Animator animator;
    private Rigidbody2D rb;
    private BoxCollider2D col;
    private bool isGrounded;
    private bool cachedQueryStartInColliders;

    private float time;

    private enum WeaponStage
    {
        Stage1,
        Stage2
    }

    private void Start()
    {
        //Initializes variables
        gameManager = GameManager.Instance; //Finds Singleton
        weaponStage = WeaponStage.Stage1; // Sets Weapon Stage to stage 1
        doStage2SecondaryAttack = false;
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        cachedQueryStartInColliders = Physics2D.queriesStartInColliders;
        currentMana = maxMana;

        sprintAction = playerInput.currentActionMap.FindAction("Sprint"); // Connects Sprint Action to Sprint Input
        jumpAction = playerInput.currentActionMap.FindAction("Jump"); // Connects Jump Action to Jump Input
        primaryAttackAction = playerInput.currentActionMap.FindAction("Primary Attack"); // Connects Primary Attack Action to Primary Attack Input
        secondaryAttackAction = playerInput.currentActionMap.FindAction("Secondary Attack"); // Connects Secondary Attack Action to Secondary Attack Input
        stageChangeAction = playerInput.currentActionMap.FindAction("Weapon Stage Change"); // Connects Weapon Stage Change Button to Weapon Stage Change Input

        // Subscribes actions to methods when start and cancel actions are detected
        playerInput.currentActionMap.FindAction("Move").performed += context => horizontalMove = context.ReadValue<float>();
        playerInput.currentActionMap.FindAction("Move").canceled += context => horizontalMove = 0f;

        jumpAction.started += Jump_Started;

        primaryAttackAction.started += Primary_Attack_Started;
        secondaryAttackAction.started += Secondary_Attack_Started;
        stageChangeAction.started += Stage_Change_Started;

        StartCoroutine(RegenMana());
    }

    float lastVerticalVelocity = 0;
    private void Update()
    {
        //Will be used for buffering and coyote time
        time += Time.deltaTime;


        //if (lastVerticalVelocity > 0 && currentMovement.y < 0)
            //currentMovement.y += 10;
        lastVerticalVelocity = currentMovement.y;

    }

    private void FixedUpdate()
    {
        //Handles Player Movement
        HandleCollisions();
        HandleJump();
        HandleMovement();
        HandleGravity();
        ApplyMovement();

        //Creates enlarging circle around player
        if (doStage2SecondaryAttack)
        {
            //Raycasts circle around player
            //Subtracts the game time from the time the attack was pressed, up to the max size
            enemyHits = Physics2D.CircleCastAll(transform.position, Mathf.Min( (time - timeSecondaryAttackWasPressed ) * growthModifierSecondaryAttackStage2 + secondaryAttackStage2StartSize, 
                secondaryAttackStage2MaxSize), Vector2.zero, 0f, attackLayer);

            for (int i = 0; i < enemyHits.Length; i++)
            {
                if (enemyHits[i].collider.gameObject.TryGetComponent(out GenericEnemy enemy))
                {
                    //TODO CHANGE!! OVERPOWERED! (All enemies within range take 60 damage PER SECOND
                    enemy.TakeDamage(1);
                    print("ENEMY HIT");
                }
            }
        }
    }

    //Initializes jumping variables
    private float timeLeftFromGround = 0f; // For coyote jump
    private float timeJumpWasPressed = 0f; // For buffer jump
    private bool canJump; // If the player can jump or not
    private bool jumpHeld; // If the jump button was held or not
    private bool canCoyoteJump; // If the player can perform a coyote jump
    private bool canBufferJump; // If the player can peform a buffer jump
    private bool endedJumpEarly; // If the jump ended early

    /**
     * Raycasts to check if player hit a ceiling or the ground
     */
    private void HandleCollisions()
    {
        //Updates raycast
        Physics2D.queriesStartInColliders = false;

        //Raycasts
        bool groundCollision = Physics2D.BoxCast(col.bounds.center, col.size, 0, Vector2.down, 0.05f, rayCastLayer);
        bool ceilingCollision = Physics2D.BoxCast(col.bounds.center, col.size, 0, Vector2.up, 0.05f, rayCastLayer);

        //Collided with a ceiling
        if (ceilingCollision) currentMovement.y = Mathf.Min(0, currentMovement.y);

        //Landed on the ground
        if (!isGrounded && groundCollision)
        {
            isGrounded = true;
            animator.SetBool("Grounded",true);
            currentMovement.y = 0;
            canCoyoteJump = true;
            canBufferJump = true;
            endedJumpEarly = false;

        }

        //Left the ground
        else if (isGrounded && !groundCollision)
        {
            isGrounded = false;
            animator.SetBool("Grounded", false);
            timeLeftFromGround = time;
        }

        Physics2D.queriesStartInColliders = cachedQueryStartInColliders;
    }

    /**
     * Handles the player jumping
     * If a jump is valid and the jump input is detected, calls the Jump method
     */
    private void HandleJump()
    {
        //If the jump button was held
        if (jumpAction.ReadValue<float>() > 0)
            jumpHeld = true;
        else
            jumpHeld = false;

        //Checks if the jump was ended early or not
        if (!endedJumpEarly && !isGrounded && !jumpHeld && rb.velocity.y > 0) endedJumpEarly = true;

        //If player cannot jump and cannot buffer a jump, do nothing
        bool hasBufferJump = canBufferJump && time < timeJumpWasPressed + bufferJumpTime;
        if (!canJump && !hasBufferJump) return;

        bool hasCoyoteJump = canCoyoteJump && !isGrounded && time < timeLeftFromGround + coyoteJumptime;
        if (isGrounded || hasCoyoteJump) Jump();

        canJump = false;
    }

    /**
     * Executes a jump
     */
    private void Jump()
    {
        //Resets variables
        endedJumpEarly = false;
        canBufferJump = false;
        canCoyoteJump = false;
        timeJumpWasPressed = 0;

        //Jumps
        animator.SetTrigger("Jump");
        currentMovement.y = jumpPower;
    }

    /**
     * Handles Horizontal Movement
     * Accelerates or decelerates player to either max velocity or 0 velocity
     */
    private void HandleMovement()
    {
        //Changes horizontal velocity if sprinting
        float speedMod = sprintAction.ReadValue<float>() > 0 ? sprintMultiplier : 1f;

        //Gets velocities for each direction
        float horizontalSpeed = horizontalMove * speed * speedMod;

        //If declerating, decelerate to 0 horizontal velocity
        if (horizontalMove == 0f)
        {
            //If ground and air deceleration is different, use the appropiate one.
            float deceleration = isGrounded ? groundDeceleration : airDeceleration;
            currentMovement.x = Mathf.MoveTowards(currentMovement.x, 0f, deceleration * Time.fixedDeltaTime * gameManager.expectedFrameRate);
        }
        //If Accelerating, accelerate to horizontalSpeed
        else
        {
            currentMovement.x = Mathf.MoveTowards(currentMovement.x, horizontalSpeed, acceleration * Time.fixedDeltaTime * gameManager.expectedFrameRate);
        }
    }

    /**
     * Handles the gravity the player is affected by
     */
    private void HandleGravity()
    {
        //If player is on the ground and not attempting to jump, apply grounding force to keep player on ground
        if (isGrounded && currentMovement.y <= 0f)
        {
            currentMovement.y = -groundingForce;
        }
        //If player is in the air, apply gravity
        else
        {
            //Updates gravity based on if the jump was ended early or not
            float usedGravity = endedJumpEarly && currentMovement.y > 0 ? gravity * endedJumpEarlyGravityModifier : gravity;

            //Increases gravity if player is falling
            usedGravity = rb.velocity.y < 0 ? usedGravity * fallGravityModifier : usedGravity;

            //Accelerates player downwards using gravity until player reaches terminal velocity / maxFallSpeed
            currentMovement.y = Mathf.MoveTowards(currentMovement.y, -maxFallSpeed, usedGravity * Time.fixedDeltaTime * gameManager.expectedFrameRate);
        }
    }

    /**
     * Applies all movement to the rigidbody to move the player
     */
    private void ApplyMovement()
    {
        if (isGrounded && currentMovement.x != 0)
            animator.SetBool("Running", true);
        else
            animator.SetBool("Running", false);
        rb.velocity = currentMovement;

        //Makes player face the direction they are moving
        if (rb.velocity.x < 0)
            spriteRenderer.flipX = true;
        else if (rb.velocity.x > 0)
            spriteRenderer.flipX = false;
    }

    /**
     * Called when the jump action is detected
     */
    private void Jump_Started(InputAction.CallbackContext obj)
    {
        canJump = true;
        timeJumpWasPressed = time;
    }

    private RaycastHit2D[] enemyHits;

    //Cooldown timers
    private float timePrimaryAttackWasPressed = 0;
    private float timeSecondaryAttackWasPressed = 0;

    /**
     * Called when attack input is detected
     */
    private void Primary_Attack_Started(InputAction.CallbackContext obj)
    {
        //Stage 1 Primary Attack
        if (weaponStage == WeaponStage.Stage1)
        {
            //Checks if weapon is on cooldown
            if (time > timePrimaryAttackWasPressed + primaryAttackStage1Cooldown || timePrimaryAttackWasPressed == 0)
            {
                //Checks if player has enough mana
                if (currentMana >= primaryAttackStage1Cost)
                {
                    currentMana -= primaryAttackStage1Cost;
                    animator.SetTrigger("Slash");
                    print("STAGE 1 -- Primary Attack");
                    timePrimaryAttackWasPressed = time;

                    //Turns off stage 2 secondary attack
                    doStage2SecondaryAttack = false;

                    //Swing Dagger
                    if (spriteRenderer.flipX)
                        enemyHits = Physics2D.CircleCastAll(leftAttackTransform.position, stage1AttackRange, Vector2.left, 0f, attackLayer);
                    else
                        enemyHits = Physics2D.CircleCastAll(rightAttackTransform.position, stage1AttackRange, Vector2.right, 0f, attackLayer);

                    //Check for everything that was hit by the dagger
                    for (int i = 0; i < enemyHits.Length; i++)
                    {
                        if (enemyHits[i].collider.gameObject.CompareTag("Projectile"))
                        {
                            EnemyProjectile projectile = enemyHits[i].collider.gameObject.GetComponent<EnemyProjectile>();
                            if (projectile != null)
                            {
                                projectile.FreezeProjectile();
                                print("FREEZE PROJECTILE");
                            }
                        }
                        if (enemyHits[i].collider.gameObject.TryGetComponent(out GenericEnemy enemy))
                        {
                            enemy.TakeDamage(1);
                            print("ENEMY HIT");
                        }
                    }
                }
            }
        }
        //Stage 2 Primary Attack
        else if (weaponStage == WeaponStage.Stage2)
        {
            //Checks if weapon is on cooldown
            if (time > timePrimaryAttackWasPressed + primaryAttackStage2Cooldown || timePrimaryAttackWasPressed == 0)
            {
                //Checks if player has enough mana
                if (currentMana >= primaryAttackStage2Cost)
                {
                    currentMana -= primaryAttackStage2Cost;

                    animator.SetTrigger("Slash_2");
                    print("STAGE 2 -- Primary Attack");
                    timePrimaryAttackWasPressed = time;

                    //Turns off stage 2 secondary attack
                    doStage2SecondaryAttack = false;

                    //Swing Sword
                    if (spriteRenderer.flipX)
                        enemyHits = Physics2D.CircleCastAll(leftAttackTransform.position, stage2AttackRange, Vector2.left, 0f, attackLayer);
                    else
                        enemyHits = Physics2D.CircleCastAll(rightAttackTransform.position, stage2AttackRange, Vector2.right, 0f, attackLayer);

                    //Check for everything that was hit by the dagger
                    for (int i = 0; i < enemyHits.Length; i++)
                    {
                        if (enemyHits[i].collider.gameObject.CompareTag("Projectile"))
                        {
                            EnemyProjectile projectile = enemyHits[i].collider.gameObject.GetComponent<EnemyProjectile>();
                            if (projectile != null)
                            {
                                projectile.FreezeProjectile();
                                print("FREEZE PROJECTILE");
                            }
                        }
                        if (enemyHits[i].collider.gameObject.TryGetComponent(out GenericEnemy enemy))
                        {
                            enemy.TakeDamage(3);
                            print("ENEMY HIT");
                        }
                    }
                }
            }
        }
    }

    private void Secondary_Attack_Started(InputAction.CallbackContext obj)
    {
        //Stage 1 Secondary Attack
        if (weaponStage == WeaponStage.Stage1)
        {
            //Checks if weapon is on cooldown
            if (time > timeSecondaryAttackWasPressed + secondaryAttackStage1Cooldown || timeSecondaryAttackWasPressed == 0)
            {
                //Checks if player has enough mana
                if (currentMana >= secondaryAttackStage1Cost)
                {
                    currentMana -= secondaryAttackStage1Cost;

                    timeSecondaryAttackWasPressed = time;
                    print("STAGE 1 -- Secondary Attack");
                    animator.SetTrigger("Secondary");

                    //Turns off stage 2 secondary attack
                    doStage2SecondaryAttack = false;

                    //Raycasts circle around player
                    //Subtracts the game time from the time the attack was pressed, up to the max size
                    enemyHits = Physics2D.CircleCastAll(transform.position, secondaryAttackStage1Size, Vector2.zero, 0f, attackLayer);

                    for (int i = 0; i < enemyHits.Length; i++)
                    {
                        if (enemyHits[i].collider.gameObject.TryGetComponent(out GenericEnemy enemy))
                        {
                            //TODO CHANGE!! OVERPOWERED! (All enemies within range take 60 damage PER SECOND
                            enemy.TakeDamage(1);
                            print("ENEMY HIT");
                        }
                    }
                }
            }
        }
        //Stage 2 Secondary Attack
        else if (weaponStage == WeaponStage.Stage2)
        {
            //Checks if weapon is on cooldown
            if (time > timeSecondaryAttackWasPressed + secondaryAttackStage2Cooldown || timeSecondaryAttackWasPressed == 0 || doStage2SecondaryAttack)
            {
                //Checks if player has enough mana
                if (currentMana >= secondaryAttackStage2Cost)
                {
                    timeSecondaryAttackWasPressed = time;
                    //Toggles attack
                    doStage2SecondaryAttack = doStage2SecondaryAttack ? false : true;

                    if (doStage2SecondaryAttack)
                    {
                        print("TURNED ON STAGE 2 -- Secondary Attack");
                        animator.SetTrigger("Aura");
                        StartCoroutine(DrainMana());
                    }
                    else
                        print("TURNED OFF STAGE 2 -- Secondary Attack");
                } else
                {
                    doStage2SecondaryAttack = false;
                }
            }
        }
    }

    /**
     * Updates the weapon stage change when detected
     */
    private void Stage_Change_Started(InputAction.CallbackContext obj)
    {
        //If value is less than 1, go to stage 1
        if (stageChangeAction.ReadValue<float>() < 1)
        {
            print("CHANGED TO STAGE 1");
            animator.SetTrigger("Stage_1");
            weaponStage = WeaponStage.Stage1;

            //Turns off stage 2 secondary attack
            doStage2SecondaryAttack = false;
        }
        //If value is greater than one, go to stage 2
        else if (stageChangeAction.ReadValue<float>() > 1)
        {
            print("CHANGED TO STAGE 2");
            animator.SetTrigger("Stage_2");
            weaponStage = WeaponStage.Stage2;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (weaponStage == WeaponStage.Stage1)
        {
            Gizmos.color = Color.green;
            if (spriteRenderer.flipX)
                Gizmos.DrawWireSphere(leftAttackTransform.position, stage1AttackRange);
            else
                Gizmos.DrawWireSphere(rightAttackTransform.position, stage1AttackRange);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, secondaryAttackStage1Size);
        }
        
        if (weaponStage == WeaponStage.Stage2)
        {
            Gizmos.color = Color.blue;
            if (spriteRenderer.flipX)
                Gizmos.DrawWireSphere(leftAttackTransform.position, stage2AttackRange);
            else
                Gizmos.DrawWireSphere(rightAttackTransform.position, stage2AttackRange);
        }

        if (doStage2SecondaryAttack)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, Mathf.Min((time - timeSecondaryAttackWasPressed) 
                * growthModifierSecondaryAttackStage2 + secondaryAttackStage2StartSize, secondaryAttackStage2MaxSize));
        }
    }

    public int GetMana()
    {
        return currentMana;
    }

    IEnumerator RegenMana()
    {
        while (true)
        {
            while (!doStage2SecondaryAttack)
            {
                if (currentMana < maxMana)
                {
                    currentMana += manaRegeneration;

                    if (currentMana > maxMana)
                        currentMana = maxMana;
                }
                yield return new WaitForSeconds(1);
            }

            while (doStage2SecondaryAttack)
            {
                yield return new WaitForSeconds(1);
            }
        }
    }

    IEnumerator DrainMana()
    {
        while (doStage2SecondaryAttack)
        {
            currentMana -= secondaryAttackStage2Cost;
            if (currentMana < 0)
            {
                currentMana = 0;
                doStage2SecondaryAttack = false;
                yield return null;
            }
            yield return new WaitForSeconds(1);
        }
    }
}