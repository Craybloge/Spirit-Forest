using UnityEngine;

// Thomas - Fait les déplacement du joueurs

public class PlayerMovement : MonoBehaviour
{

    public float moveSpeed;
    public float jumpForce;
    public float groundCheckRadius;
    private float horizontalMovement;
    public float facingCoef = 1f;
    public int jumpStamina = 10;

    public bool isJumping;
    public bool isGrounded;
    public bool isNearWall;
    public int wallJump = 0;
    //public bool faceRight = true;
    //public bool combatMode = false;

    public PlayerStamina playerStamina;
    public PlayerCombat combat;
    public Transform groundCheck;
    public LayerMask collisionGroundLayers;
    public LayerMask collisionWallLayers;
    public Rigidbody2D rb;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public CapsuleCollider2D playerCollider;
    public Vector3 velocity = Vector3.zero;
    public static PlayerMovement instance;
    public AudioSource audioSrc;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance de PlayerMovement dans la scène");
            return;
        }

        instance = this;
    }

    private void Update()
    {
        if (isGrounded == true)
        {
            wallJump = 0;
        }


        if ((rb.velocity.x > 0.3f && isGrounded == true) || (rb.velocity.x < -0.3f && isGrounded == true))
        {
            if (!audioSrc.isPlaying)
            {
                audioSrc.Play();
            }
        }
        else
        {
            audioSrc.Stop();
        }

        horizontalMovement = Input.GetAxis("Horizontal") * moveSpeed * Time.fixedDeltaTime * facingCoef;

        if (Input.GetButtonDown("Jump") && ((isGrounded == true) || (isNearWall && wallJump < 1)) && (playerStamina.CurrentStamina >= jumpStamina)) //Jump correspond par defaut à la barre espace
        {
            isJumping = true;
            if(isNearWall == true)
            {
              wallJump ++;
            }
            playerStamina.LoseStamina(jumpStamina);
        }

        if (isGrounded == false)
        {
            animator.SetBool("Jump", true);
        }
        else
        {
            animator.SetBool("Jump", false);
        }

        Flip(rb.velocity.x);

        float characterVelocity = Mathf.Abs(rb.velocity.x);             //rb.velocity.x = vitesse du personnage sur axe X
        animator.SetFloat("Speed", characterVelocity);

    }

    public void MovePlayer(float _horizontalMovement)
    {
        Vector3 targetVelocity = new Vector2(_horizontalMovement, rb.velocity.y);
        rb.velocity = Vector3.SmoothDamp(rb.velocity, targetVelocity, ref velocity, .05f);

        if (isJumping == true)
        {
            rb.AddForce(new Vector2(0f, jumpForce));
            isJumping = false;
        }
    }
    void FixedUpdate()//FIXED UPDATE S'UTILISE SEULEMENT POUR LES OPERATIONS DE PHYSIQUE (pas d'input ou quoi que ce soit d'autre)
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, collisionGroundLayers);
        MovePlayer(horizontalMovement);                                 //Si pb de déplacement remettre cette ligne dans fixedUpdate

        isNearWall = playerCollider.IsTouchingLayers(collisionWallLayers);
    }

    void Flip(float _velocity)
    {
        if (_velocity > 0.1f)
        {
            spriteRenderer.transform.localScale = new Vector3(0.21f, 0.17f, 1);
        }
        else if (_velocity < -0.1f)
        {
            spriteRenderer.transform.localScale = new Vector3(-0.21f, 0.17f, 1);
        }
    }

    public void SlowPlayer(float slow)//Pas encore testé
    {
        moveSpeed *= (1 / slow);
    }

    public void SpeedPlayer(float speed)//Pas encore testé
    {
        moveSpeed *= speed;
    }
}
