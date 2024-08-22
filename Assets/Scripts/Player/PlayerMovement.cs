using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
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
    [SerializeField] private float speed;
    [SerializeField] private float sprintMultiplier;
    [SerializeField] private float acceleration, groundDeceleration, airDeceleration;
    [Header("Jump Movement")]
    [SerializeField] private float jumpPower;
    [SerializeField] private float gravity, maxFallSpeed;
    [Tooltip("The force applied to keep the player on the ground")]
    [SerializeField] private float groundingForce;

    GameManager gameManager;
    private InputAction sprintAction;
    private InputAction jumpAction;

    private float horizontalMove;
    private Vector2 currentMovement;

    private Rigidbody2D rb;
    private CircleCollider2D col;
    private bool isGrounded;

    private float time;

    private void Start()
    {
        //Initializes variables
        gameManager = GameManager.Instance; //Finds Singleton
        playerInput = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<CircleCollider2D>();

        sprintAction = playerInput.currentActionMap.FindAction("Sprint"); // Connects Sprint Action to Sprint Input
        jumpAction = playerInput.currentActionMap.FindAction("Jump"); // Connects Jump Action to Jump Input

        // Subscribes actions to methods when start and cancel actions are detected
        playerInput.currentActionMap.FindAction("Move").performed += context => horizontalMove = context.ReadValue<float>();
        playerInput.currentActionMap.FindAction("Move").canceled += context => horizontalMove = 0f;
    }

    private void Update()
    {
        //Will be used for buffering and coyote time
        time += Time.deltaTime;
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

    /**
     * Raycasts to check if player hit a ceiling or the ground
     */
    private void HandleCollisions()
    {
        //Raycasts
        bool groundCollision = Physics2D.CircleCast(col.bounds.center, col.radius, Vector2.down, 0.05f, rayCastLayer);
        bool ceilingCollision = Physics2D.CircleCast(col.bounds.center, col.radius, Vector2.up, 0.05f, rayCastLayer);

        //Collided with a ceiling
        if (ceilingCollision) currentMovement.y = Mathf.Min(0, currentMovement.y);

        //Landed on the ground
        if (!isGrounded && groundCollision)
        {
            isGrounded = true;
        }

        //Left the ground
        else if (isGrounded && !groundCollision)
        {
            isGrounded = false;
        }
    }

    /**
     * Handles the player jumping
     */
    private void HandleJump()
    {
        //If player is grounded and jump input is triggered, jump
        if (isGrounded && jumpAction.ReadValue<float>() > 0)
        {
            currentMovement.y = jumpPower;
            print("JUMP: " + currentMovement.y);
        }
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
            //Accelerates player downwards using gravity until player reaches terminal velocity / maxFallSpeed
            currentMovement.y = Mathf.MoveTowards(currentMovement.y, -maxFallSpeed, gravity * Time.fixedDeltaTime * expectedFrameRate);
        }
    }

    /**
     * Applies all movement to the rigidbody to move the player
     */
    private void ApplyMovement() { rb.velocity = currentMovement; print(rb.velocity); }
}