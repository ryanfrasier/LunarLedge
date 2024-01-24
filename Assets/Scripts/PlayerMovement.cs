using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask wallLayer;
    private Rigidbody2D body;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private float wallJumpCooldown;
    private float horizontalInput;

    private void Awake()
    {
        // Grabs references for Rigidbody and Animator from the game object.
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        // Flip player when facing left/right.
        if (horizontalInput > 0.01f)
            transform.localScale = Vector3.one;
        else if (horizontalInput < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);

        // Sets animation parameters.
        anim.SetBool("run", horizontalInput != 0);
        anim.SetBool("grounded", isGrounded());

        //Wall jump logic
        if(wallJumpCooldown > 0.2f)
        {
            body.velocity = new Vector2(horizontalInput * speed, body.velocity.y);

            if(onWall() && !isGrounded())
            {
                body.gravityScale = 1; // Reduced gravity on the wall
                if(body.velocity.y < 0) // Apply only when falling down
                {
                    body.velocity = new Vector2(body.velocity.x, -0.5f); // Small downward velocity
                }
            }
            else
                body.gravityScale = 5;

            if (Input.GetKey(KeyCode.Space))
                Jump();
        }
        else
            wallJumpCooldown += Time.deltaTime;
    }

    private void Jump()
    {
        if(isGrounded())
        {
            body.velocity = new Vector2(body.velocity.x, jumpPower);
            anim.SetTrigger("jump");
        }
        else if(onWall() && !isGrounded())
        {
            Vector2 jumpDirection;
            if(horizontalInput == 0)
            {
                // Jump away from the wall
                jumpDirection = new Vector2(-Mathf.Sign(transform.localScale.x), 0).normalized;
            }
            else
            {
                // Diagonal jump away from the wall
                jumpDirection = new Vector2(-Mathf.Sign(transform.localScale.x), 1).normalized;
            }

            body.velocity = jumpDirection * jumpPower; // Use the same jump power for both cases

            // Flip the player's orientation after the jump
            transform.localScale = new Vector3(-Mathf.Sign(transform.localScale.x), transform.localScale.y, transform.localScale.z);

            wallJumpCooldown = 0;
        }
    }

    private bool isGrounded()
    {
        // Performs a BoxCast to check if the player is grounded.
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.down, 0.1f, groundLayer);
        return raycastHit.collider != null;
    }

    private bool onWall()
    {
        // Performs a BoxCast to check if the player is on a wall.
        RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), 0.1f, wallLayer);
        return raycastHit.collider != null;
    }
    public bool canAttack()
    {
        return horizontalInput == 0 && isGrounded() && !onWall();
    }
}
