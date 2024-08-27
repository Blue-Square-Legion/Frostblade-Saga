using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //Player variables
    [Header("Player")]
    [SerializeField] private GameObject player;
    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private LayerMask rayCastLayer;
    [SerializeField] private Camera cam;
    [SerializeField] private InputActionAsset playerControls;
    [SerializeField] private int expectedFrameRate;
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
    [SerializeField] private Transform attackAreaTransformRight;
    [SerializeField] private Transform attackAreaTransformLeft;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private LayerMask attackLayer;

    GameManager gameManager;
    private InputAction sprintAction;
    private InputAction jumpAction;
    private InputAction meleeAttackAction;

    private float horizontalMove;
    private Vector2 currentMovement;

    private Rigidbody2D rb;
    private BoxCollider2D col;
    private SpriteRenderer spriteRenderer;
    private bool isGrounded;
    private bool cachedQueryStartInColliders;

    private float time;

    private void Start()
    {
        //Initializes variables
        gameManager = GameManager.Instance; //Finds Singleton
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        cachedQueryStartInColliders = Physics2D.queriesStartInColliders;

        sprintAction = playerInput.currentActionMap.FindAction("Sprint"); // Connects Sprint Action to Sprint Input
        jumpAction = playerInput.currentActionMap.FindAction("Jump"); // Connects Jump Action to Jump Input
        meleeAttackAction = playerInput.currentActionMap.FindAction("Melee Attack"); // Connects Melee Attack Action to Melee Attack Input

        // Subscribes actions to methods when start and cancel actions are detected
        playerInput.currentActionMap.FindAction("Move").performed += context => horizontalMove = context.ReadValue<float>();
        playerInput.currentActionMap.FindAction("Move").canceled += context => horizontalMove = 0f;

        jumpAction.started += Jump_Started;

        meleeAttackAction.started += Melee_Attack_Started;
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
            currentMovement.y = 0;
            canCoyoteJump = true;
            canBufferJump = true;
            endedJumpEarly = false;

        }

        //Left the ground
        else if (isGrounded && !groundCollision)
        {
            isGrounded = false;
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
            currentMovement.x = Mathf.MoveTowards(currentMovement.x, 0f, deceleration * Time.fixedDeltaTime * expectedFrameRate);
        }
        //If Accelerating, accelerate to horizontalSpeed
        else
        {
            currentMovement.x = Mathf.MoveTowards(currentMovement.x, horizontalSpeed, acceleration * Time.fixedDeltaTime * expectedFrameRate);
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
            currentMovement.y = Mathf.MoveTowards(currentMovement.y, -maxFallSpeed, usedGravity * Time.fixedDeltaTime * expectedFrameRate);
        }
    }

    /**
     * Applies all movement to the rigidbody to move the player
     */
    private void ApplyMovement()
    {
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

    /**
     * Called when attack input is detected
     */
    private void Melee_Attack_Started(InputAction.CallbackContext obj)
    {
        if (spriteRenderer.flipX)
        {
            enemyHits = Physics2D.CircleCastAll(attackAreaTransformLeft.position, attackRange, Vector2.left, 0f, attackLayer);
        }
        else
        {
            enemyHits = Physics2D.CircleCastAll(attackAreaTransformRight.position, attackRange, Vector2.right, 0f, attackLayer);
        }

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

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackAreaTransformLeft.position, attackRange);
        Gizmos.DrawWireSphere(attackAreaTransformRight.position, attackRange);
    }
}