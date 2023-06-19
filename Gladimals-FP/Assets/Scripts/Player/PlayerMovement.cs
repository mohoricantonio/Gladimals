using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed;
    public float rotationSpeed = 10f;
    private float horizontalInput;
    private float verticalInput;

    private Vector3 moveDirection;
    private Vector3 rotateDirection;
    private Rigidbody rb;

    public float drag;
    public LayerMask whatIsGround;
    private bool isGrounded;
    public float playerHeight;

    public float jumpForce;
    public float jumpCooldown;
    private bool readyToJump;
    private bool isDoubleJumping;

    private bool canDash;
    private bool isDashing;
    public float dashingPower;
    public float dashingTime;
    public float dashingCooldown;
    private TrailRenderer trailRenderer;

    public Camera playerCamera;

    private Animator anim;
    private string animate;
    private bool isMoving;

    private bool weaponDrawn;

    public AudioClip jumpSound;
    public AudioClip dashSound;
    public AudioClip stepSound;
    private AudioSource audioSource;

    private bool isPlayingStepSound;
    private float stepSoundDelay = 0.3f;
    public bool canMoove = true;
    public float canMooveCooldown = 0;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        isMoving = false;
        animate = "";
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;
        rb.drag = drag;
        canDash = true;
        trailRenderer = GetComponent<TrailRenderer>();
        isDoubleJumping = false;
        audioSource = GetComponent<AudioSource>();

        anim.SetBool("CanMove", true);
        anim.SetBool("Weapon drawn", false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isDashing || !canMoove)
        {
            return;
        }
        
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.1f, whatIsGround);
        if (isGrounded) isDoubleJumping = false;
        GetInput();

        ControlSpeed();
    }

    private void FixedUpdate()
    {
        if (isDashing || !canMoove)
        {
            if (canMooveCooldown > 0)
            {
                canMooveCooldown -= Time.deltaTime;
            }
            else
            {
                canMoove = true;
            }
            return;
        }
        Move();
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (horizontalInput == 0 && verticalInput == 0)
        {
            isMoving = false;
        }
            

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && readyToJump)
        {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJumpCooldown), jumpCooldown);
        }
        if (Input.GetKeyDown(KeyCode.Space) && !isGrounded && !isDoubleJumping)
        {
            Jump();
            isDoubleJumping = true;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }

    }
    private void Move()
    {

        if (isMoving == false || isGrounded == false)
        {
            ResetAnimations();
        }

        moveDirection = Quaternion.Euler(0, playerCamera.transform.eulerAngles.y, 0) * new Vector3(horizontalInput, 0, verticalInput);
        if (verticalInput == -1)
        {
            animate = "RunBack";
            rotateDirection = Quaternion.Euler(0, playerCamera.transform.eulerAngles.y - 180, 0) * new Vector3(horizontalInput, 0, verticalInput);
        }
        else if (verticalInput == 0 && horizontalInput == 1)
        {
            animate = "RunRight";
            rotateDirection = Quaternion.Euler(0, playerCamera.transform.eulerAngles.y - 90, 0) * new Vector3(horizontalInput, 0, verticalInput);
        }
        else if (verticalInput == 0 && horizontalInput == -1)
        {
            animate = "RunLeft";
            rotateDirection = Quaternion.Euler(0, playerCamera.transform.eulerAngles.y + 90, 0) * new Vector3(horizontalInput, 0, verticalInput);
        }
        else
        {
            animate = "RunFD";
            rotateDirection = Quaternion.Euler(0, playerCamera.transform.eulerAngles.y, 0) * new Vector3(horizontalInput, 0, verticalInput);
        }

        Vector3 movementDirection = moveDirection.normalized;
        Vector3 rotationDirection = rotateDirection.normalized;


        try
        {
            if (anim.GetCurrentAnimatorClipInfo(1)[0].clip.name == "PowerSlash" ||
                anim.GetCurrentAnimatorClipInfo(1)[0].clip.name == "Slash3")
            {
                Vector3 move = Quaternion.Euler(0, playerCamera.transform.eulerAngles.y, 0) * new Vector3(0, 0, 1);
                rb.AddForce(move.normalized * 20f, ForceMode.Force);
            }
        }
        catch (System.Exception)
        { }

        if (anim.GetBool("CanMove"))
        {
            rb.AddForce(movementDirection * movementSpeed * 10f, ForceMode.Force);

            if (movementDirection != Vector3.zero)
            {
                Quaternion desiredRotation = Quaternion.LookRotation(rotationDirection, Vector3.up);

                transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationSpeed * Time.deltaTime);
                isMoving = true;
                if (isGrounded)
                {
                    switch (animate)
                    {
                        case "RunBack":
                            anim.SetBool("RunBack", true);
                            anim.SetBool("RunRight", false);
                            anim.SetBool("RunLeft", false);
                            anim.SetBool("RunFD", false);
                            break;
                        case "RunRight":
                            anim.SetBool("RunBack", false);
                            anim.SetBool("RunRight", true);
                            anim.SetBool("RunLeft", false);
                            anim.SetBool("RunFD", false);
                            break;
                        case "RunLeft":
                            anim.SetBool("RunBack", false);
                            anim.SetBool("RunRight", false);
                            anim.SetBool("RunLeft", true);
                            anim.SetBool("RunFD", false);
                            break;
                        case "RunFD":
                            anim.SetBool("RunBack", false);
                            anim.SetBool("RunRight", false);
                            anim.SetBool("RunLeft", false);
                            anim.SetBool("RunFD", true);
                            break;
                        default:
                            break;
                    }

                    if (!isPlayingStepSound && (anim.GetBool("RunBack") || anim.GetBool("RunRight") || anim.GetBool("RunLeft") || anim.GetBool("RunFD")))
                    {
                        StartCoroutine(PlayStepSoundWithDelay());
                    }
                }
            }
            else
            {
                isMoving = false;
            }
        }
        
    }

    private void ResetAnimations()
    {
        animate = "";
        anim.SetBool("RunBack", false);
        anim.SetBool("RunRight", false);
        anim.SetBool("RunLeft", false);
        anim.SetBool("RunFD", false);
    }

    private void ControlSpeed()
    {
        Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);


        if (flatVelocity.magnitude > movementSpeed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * movementSpeed;
            rb.velocity = new Vector3(limitedVelocity.x, rb.velocity.y, limitedVelocity.z);
        }
    }

    private IEnumerator PlayJumpSound()
    {
        yield return new WaitForSeconds(0.3f);
        audioSource.PlayOneShot(jumpSound);
    }

    private void Jump()
    {
        ResetAnimations();
        anim.SetTrigger("Jump");
        StartCoroutine(PlayJumpSound());
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJumpCooldown()
    {
        readyToJump = true;
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        rb.useGravity = false;
        moveDirection = Quaternion.Euler(0, playerCamera.transform.eulerAngles.y, 0) * new Vector3(horizontalInput, 0, verticalInput);
        rb.AddForce(moveDirection * dashingPower * movementSpeed);
        audioSource.PlayOneShot(dashSound);
        trailRenderer.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        rb.useGravity = true;
        trailRenderer.emitting = false;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    private IEnumerator PlayStepSoundWithDelay()
    {
        isPlayingStepSound = true;
        audioSource.PlayOneShot(stepSound);
        yield return new WaitForSeconds(stepSoundDelay);
        isPlayingStepSound = false;
    }
    public void cantMoove(int time)
    {
        canMoove = false;
        canMooveCooldown = time;
    }
}
