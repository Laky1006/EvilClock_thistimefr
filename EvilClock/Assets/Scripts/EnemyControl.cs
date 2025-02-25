using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    public Transform player;
    public float chaseSpeed = 2f;
    public float jumpForce = 2f;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    public bool isGrounded;
    public bool shouldJump;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Grounded??
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 1f, groundLayer);

        // Player direction
        float direction = Mathf.Sign(player.position.x - transform.position.x);

        bool isPlayerAbove = Physics2D.Raycast(transform.position, Vector2.up, 3f, 1 << player.gameObject.layer);
        bool isPlayerUnder = Physics2D.Raycast(transform.position, Vector2.down, 3f, 1 << player.gameObject.layer);

        //if grounded
       
        // Run after player 
        rb.linearVelocity = new Vector2(direction * chaseSpeed, rb.linearVelocity.y);

        // checks if it has ground in front of it (or if needs to jump over a gap)
        RaycastHit2D groundInfornt = Physics2D.Raycast(transform.position, new Vector2(direction, 0), 2f, groundLayer);

        RaycastHit2D gapInfront = Physics2D.Raycast(transform.position + new Vector3(direction, 0, 0), Vector2.down, 2f, groundLayer);

        // Checks if there is a platform above it
        RaycastHit2D platformAbove = Physics2D.Raycast(transform.position, Vector2.up, 3f, groundLayer);

        if (isGrounded && gapInfront.collider)
        {
            shouldJump = true;
            Debug.Log("1st");
        }
        //else if (isGrounded && isPlayerAbove && platformAbove.collider)
        //{
        //    shouldJump = true;
        //    Debug.Log("2st");
        //}
        
    }

    private void FixedUpdate()
    {
        if (shouldJump)
        {
            shouldJump = false;
            Debug.Log("BOOM");
            Vector2 direction = (player.position - transform.position).normalized;

            Vector2 jumpDirection = direction * jumpForce;
            rb.AddForce(new Vector2(jumpDirection.x, jumpForce), ForceMode2D.Impulse);
        }
    }
}
