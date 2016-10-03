using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    [HideInInspector]
    private bool lastJumpPressed = false;
    [HideInInspector]
    public bool grounded = false;
    [HideInInspector]
    private bool isfacingRight = true;
    [HideInInspector]
    protected bool shouldJump = false;
    [HideInInspector]
    private bool sliding = false;
    [HideInInspector]
    private bool gliding = false;
    [HideInInspector]
    private bool climbing = false;
    [HideInInspector]
    private bool lastClimbingValue = false;
    [HideInInspector]
    private bool onLadder = false;
    [HideInInspector]
    private bool gotGrounded = false;
    [HideInInspector]
    private bool pressedGlide = false;
    [HideInInspector]
    private bool dying = false;

    [HideInInspector]
    public float speed = 0;
    [HideInInspector]
    public float ladderPosition = 0;

    public float initialSpeed;
    public float maxSpeed;
    public float jumpForce;
    public float climbSpeed;
    public Vector2 Spawn;

    [HideInInspector]
    public float incrementSpeedBy;

    [HideInInspector]
    public float decrementSpeedBy;

    public Transform topLeftCheck;
    public Transform bottomRightCheck;
    public LayerMask whatIsGround;

    [HideInInspector]
    public Animator anim;
    [HideInInspector]
    public Rigidbody2D body;

    public Timer timer;

    public BoxCollider2D box;


    public bool ShouldJump
    {
        get
        {
            return shouldJump;
        }
        set
        {
            shouldJump = value;
        }
    }

    // Use this for initialization
    public void Start()
    {
        anim = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        incrementSpeedBy = 0.1f;
        decrementSpeedBy = 1;
    }

    // FixedUpdate is called before physics updates
    public void FixedUpdate()
    {
        if (!dying)
        {
            grounded = isGrounded();
            if (!sliding && !gliding)
            {
                if (shouldJump)
                {
                    shouldJump = false;
                    body.AddForce(new Vector2(0, jumpForce));
                    grounded = false;
                    climbing = false;
                }

                anim.SetBool("Grounded", grounded);

                float move = Input.GetAxis("Horizontal");
                float vertical = Input.GetAxis("Vertical");
                if (grounded && vertical < 0) vertical = 0;
                if (vertical != 0 && onLadder)
                {
                    climbing = true;
                    transform.Translate(new Vector2(ladderPosition - transform.position.x, 0), Space.World);
                    speed = 0;
                }
                if (climbing)
                {
                    var climbvelocity = climbSpeed * vertical;
                    body.velocity = new Vector2(0, climbvelocity);
                    anim.SetFloat("ClimbSpeed", climbvelocity);
                }
                if (move != 0 && !climbing)
                {
                    if (speed == 0)
                    {
                        speed = move * initialSpeed;
                    }
                    else
                    {
                        var increment = grounded ? incrementSpeedBy : incrementSpeedBy / 2;
                        var decrement = grounded ? decrementSpeedBy : decrementSpeedBy / 2;
                        if (speed < 0 == move < 0)
                        {
                            speed += (move * increment);
                        }
                        else
                        {
                            speed += (move * decrement);
                        }
                    }
                    if (Mathf.Abs(speed) > maxSpeed)
                    {
                        speed = maxSpeed * (move / Mathf.Abs(move));
                    }
                }
                else if (grounded)
                {
                    if (speed > decrementSpeedBy)
                        speed = speed - decrementSpeedBy;
                    else if (speed < -decrementSpeedBy)
                        speed = speed + decrementSpeedBy;
                    else
                        speed = 0;
                }

                anim.SetFloat("Speed", Mathf.Abs(speed));

                if (isThereAWall() && !grounded)
                {
                    speed = 0;
                }
                body.velocity = new Vector2(speed, body.velocity.y);

                if ((body.velocity.x > 0 && !isfacingRight) || (body.velocity.x < 0 && isfacingRight))
                    flip();
                if (climbing && (!onLadder || gotGrounded))
                {
                    body.velocity = Vector2.zero;
                    climbing = false;
                }
                if (lastClimbingValue != climbing)
                {
                    if (climbing)
                    {
                        body.gravityScale = 0;
                    }
                    else
                    {
                        body.gravityScale = 1;
                    }
                }
                lastClimbingValue = climbing;
            }
        }
    }

    private IEnumerator Slide(float boostDur)
    {
        if (speed < initialSpeed + incrementSpeedBy)
        {
            speed = (isfacingRight ? initialSpeed : -initialSpeed) * 1.5f;
        }
        box.size = new Vector2(box.size.x, box.size.y / 2);
        box.offset = new Vector2(box.offset.x, box.offset.y - 1);
        float time = 0;
        sliding = true;
        yield return 0;
        while (boostDur > time)
        {
            time += Time.deltaTime;
            if (isThereAWall())
            {
                speed = 0;
            }
            body.velocity = new Vector2(speed, body.velocity.y);
            yield return 0;
        }
        while (isThereACieling())
        {
            if (Mathf.Abs(speed) <= initialSpeed) speed = isfacingRight ? initialSpeed : -initialSpeed;
            body.velocity = new Vector2(speed, body.velocity.y);
            yield return 0;
        }
        sliding = false;
        anim.SetBool("Sliding", false);
        box.size = new Vector2(box.size.x, box.size.y * 2);
        box.offset = new Vector2(box.offset.x, box.offset.y + 1);
    }

    private IEnumerator Glide()
    {
        gliding = true;
        speed = (isfacingRight ? initialSpeed : -initialSpeed) * 1.2f;
        body.gravityScale = .01f;
        body.velocity = new Vector2(speed, 0);
        yield return 0;
        while (gliding)
        {
            body.velocity = new Vector2(speed, body.velocity.y);
            yield return 0;
        }
        gliding = false;
        anim.SetBool("Gliding", false);
        body.gravityScale = 1;
    }

    private bool isThereACieling()
    {
        return Physics2D.OverlapArea(new Vector2(transform.position.x + box.bounds.extents.x - .1f, transform.position.y + 0.2f + box.bounds.size.y*2),
                                     new Vector2(transform.position.x - box.bounds.extents.x + .1f, transform.position.y + box.bounds.size.y),
                                     whatIsGround);
    }


    private bool isThereAWall()
    {
        return isfacingRight ? Physics2D.OverlapArea(new Vector2(transform.position.x + box.bounds.extents.x - 0.06f, transform.position.y + 0.1f + box.bounds.size.y),
                          new Vector2(transform.position.x + box.bounds.extents.x + 0.06f, transform.position.y + 0.02f), whatIsGround) :
                          Physics2D.OverlapArea(new Vector2(transform.position.x - box.bounds.extents.x - 0.06f, transform.position.y + 0.1f + box.bounds.size.y),
                                     new Vector2(transform.position.x - box.bounds.extents.x + 0.06f, transform.position.y + 0.02f), whatIsGround);
    }

    private bool isGrounded()
    {
        var currentGrounded = Physics2D.OverlapArea(topLeftCheck.position, bottomRightCheck.position, whatIsGround);
        gotGrounded = currentGrounded && !grounded;
        return currentGrounded;
    }


    private void flip()
    {
        isfacingRight = !isfacingRight;
        Vector3 Scale = transform.localScale;
        Scale.x *= -1;
        transform.localScale = Scale;
    }

    private void EndDeath()
    {
        Timer.Time +=10;
        dying = false;
        transform.position = Spawn;
    }

    public void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Climbable")
        {
            onLadder = true;
            ladderPosition = other.transform.position.x;
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Death"){
            dying = true;
            shouldJump = false;
            gliding = false;
            climbing = false;
            speed = 0;
            body.velocity = Vector2.zero;
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Climbable")
        {
            onLadder = false;
        }
    }


    // Update is called once per frame
    public void Update()
    {
        if (!sliding && !gliding && !dying)
        {
            if ((grounded || climbing) && Input.GetAxis("Jump") > 0 && !gotGrounded && !lastJumpPressed)
            {
                ShouldJump = true;
                lastJumpPressed = true;
            }
            else if (Input.GetAxis("Jump") <= 0)
            {
                lastJumpPressed = false;
            }
            if (grounded && Input.GetAxis("Slide") > 0)
            {
                StartCoroutine(Slide(0.4f));
            }
            if (!grounded && !isThereAWall() && Input.GetAxis("Glide") > 0 && !pressedGlide)
            {
                pressedGlide = true;
                StartCoroutine(Glide());
            }
            else if (Input.GetAxis("Glide") <= 0)
            {
                pressedGlide = false;
            }
        }
        if (gliding)
        {
            if ((grounded || isThereAWall() || Input.GetAxis("Glide") > 0) && !pressedGlide)
            {
                gliding = false;
                pressedGlide = true;
            }
            else if (Input.GetAxis("Glide") <= 0)
            {
                pressedGlide = false;
            }
        }
        anim.SetBool("Climbing", climbing);
        anim.SetBool("Sliding", sliding);
        anim.SetBool("Gliding", gliding);
        anim.SetBool("Dying", dying);
    }
}
