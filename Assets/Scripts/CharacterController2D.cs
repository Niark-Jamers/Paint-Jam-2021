using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]

public class CharacterController2D : MonoBehaviour
{
    // Move player in 2D space
    public float maxSpeed = 3.4f;
    public float gravityScale = 1.5f;
    public float acceleration = 0.125f;
    public float patination = 0.02f;
    public Camera mainCamera;

    float speed = 0;
    bool facingRight = true;
    float moveDirection = 0;
    bool isGrounded = false;
    Vector3 cameraPos;
    Rigidbody2D r2d;
    CapsuleCollider2D mainCollider;
    Transform t;
    bool jumpPressed;
    int lastJumpPressedFrame = 2000;
    Animator animator;

    [Header("Jump")]
    public float jumpHeight = 6.5f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public int jumpFrameDetectionCount = 20;

    [Header("Audio")]
    public AudioClip[] steps;
    public AudioClip jump;
    public float stepTimeout = 0.2f;
    float lastStep;

    float jumpLastTime;

    [System.NonSerialized]
    internal bool disableInputs = false;

    // Use this for initialization
    void Start()
    {
        t = transform;
        r2d = GetComponent<Rigidbody2D>();
        mainCollider = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
        r2d.freezeRotation = true;
        r2d.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        r2d.gravityScale = gravityScale;
        facingRight = t.localScale.x > 0;

        if (mainCamera)
        {
            cameraPos = mainCamera.transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Movement controls
        if (!disableInputs && (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)))
        {
            moveDirection = (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.LeftArrow)) ? -1 : 1;
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.LeftArrow))
            {
                speed = -maxSpeed;
                // if (speed > -1f)
                // {
                //     speed = speed - 0.01f;
                // }
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                speed = maxSpeed;
                // if (speed < 1f)
                // {
                //     speed = speed + 0.01f;
                // }
            }
        }
        else
        {
            speed = 0;
            moveDirection = 0;
        }

        // Change facing direction
        if (moveDirection != 0)
        {
            if (moveDirection > 0 && !facingRight)
            {
                facingRight = true;
                t.localScale = new Vector3(Mathf.Abs(t.localScale.x), t.localScale.y, transform.localScale.z);
            }
            if (moveDirection < 0 && facingRight)
            {
                facingRight = false;
                t.localScale = new Vector3(-Mathf.Abs(t.localScale.x), t.localScale.y, t.localScale.z);
            }
        } else
        {
            // if (speed > 0)
            // {
            //     speed = speed - 0.025f;
            // }
            // else if (speed < 0)
            // {
            //     speed = speed + 0.025f;
            // }
        }
        //Debug.Log(speed);

        if (speed != 0 && isGrounded)
        {
            if (Time.time - lastStep > stepTimeout)
            {
                lastStep = Time.time;
                AudioManager.instance.PlaySFX(steps[Random.Range(0, steps.Length)], 0.1f);
            }
        }

        // Jumping
        bool wantsToJump = !disableInputs && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.Z));
        if (r2d.velocity.y < 0)
            r2d.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        else if (r2d.velocity.y > 0 && !wantsToJump)
            r2d.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        
        animator.SetFloat("Velocity X", Mathf.Abs(r2d.velocity.x));
        animator.SetFloat("Velocity Y", r2d.velocity.y);

        UpdateJumpPressed();
        if (jumpPressed && isGrounded && Time.time - jumpLastTime > 0.2f)
        {
            jumpLastTime = Time.time;
            animator.SetTrigger("Jump");
            AudioManager.instance.PlaySFX(jump, 0.2f);
            r2d.velocity = new Vector2(r2d.velocity.x, jumpHeight);
        }

        // Camera follow
        if (mainCamera)
        {
            mainCamera.transform.position = new Vector3(t.position.x, cameraPos.y, cameraPos.z);
        }
    }

    void UpdateJumpPressed()
    {
        bool jump = !disableInputs && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Z));
        if (jump)
            lastJumpPressedFrame = 0;
        
        // If jump was pressed 4 frames before being grounded, we jump again
        jumpPressed = lastJumpPressedFrame <= jumpFrameDetectionCount;
        
        lastJumpPressedFrame++;
    }

    void FixedUpdate()
    {
        Bounds colliderBounds = mainCollider.bounds;
        float colliderRadius = mainCollider.size.x * 0.4f * Mathf.Abs(transform.localScale.x);
        Vector3 groundCheckPos = colliderBounds.min + new Vector3(colliderBounds.size.x * 0.5f, colliderRadius * 0.9f, 0);
        // Check if player is grounded
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckPos, colliderRadius);
        //Check if any of the overlapping colliders are not player collider, if so, set isGrounded to true
        isGrounded = false;
        if (colliders.Length > 0)
        {
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].isTrigger)
                    continue;
                if (colliders[i] != mainCollider)
                {
                    isGrounded = true;
                    break;
                }
            }
        }

        // Apply movement velocity
        r2d.velocity = new Vector2(speed, r2d.velocity.y);

        // Simple debug
        Debug.DrawLine(groundCheckPos, groundCheckPos - new Vector3(0, colliderRadius, 0), isGrounded ? Color.green : Color.red);
        Debug.DrawLine(groundCheckPos, groundCheckPos - new Vector3(colliderRadius, 0, 0), isGrounded ? Color.green : Color.red);
    }
}
