using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;


public class Player : MonoBehaviour
{
    public static Player instance;

    [HideInInspector]
    public Rigidbody2D rb;

    [Header("Character Atributes")]
    public float speed;
    public float jumpForce;
    [Range(0, 2)]
    public float forceDown;
    [Range(0, 1)]
    public float cutVelY;
    public float maxNegativeY;
    public float maxTimeJumping;
    private float timeJumping;
    public float invencibleTime;
    private float invencibleTimer;
    public float alphaMutiplier;
    private float alpha;
    private bool tookDamage;
    public bool isOnGround;
    private bool canPlay;

    public float pressTimer;
    public float pressTime;
    public float pressTimeAdd;

    public bool canMove;


    [Header("Reference")]
    public Animator an;
    public LifeUI lifeScript;
    public LayerMask ground;
    public BoxCollider2D boxCollider;
    public AudioSource audioSource;
    public Anima2D.SpriteMeshInstance[] anima2D;


    [Header("Audio")]
    public AudioClip jump;
    public AudioClip hurt;


    private bool isPressing;
    [HideInInspector]
    private Touch touch;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        transform.position = CheckPointManager.instance.playerPos;

        alpha = 1;
        canPlay = true;
        //CheckPointManager.instance.LoadPos(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (!CheckPointManager.instance.isPaused)
        {
            rb.bodyType = RigidbodyType2D.Dynamic;

            Jump();
            //Debug.Log(IsGrounded());
            Debug.DrawRay(transform.position, Vector2.down * 1, Color.magenta);

            if (tookDamage)
            {
                invencibleTimer += Time.deltaTime;
                if (invencibleTimer >= invencibleTime)
                {
                    StopAllCoroutines();
                    tookDamage = false;
                    alpha = 1;
                    invencibleTimer = 0;


                }
            }

            for (int i = 0; i < anima2D.Length; i++)
            {
                anima2D[i].color = new Color(anima2D[i].color.r, anima2D[i].color.g, anima2D[i].color.b, alpha);
            }
            
        }
        else
        {
            if (!CheckPointManager.instance.isDead)
            {
                rb.velocity = Vector2.zero;
                rb.bodyType = RigidbodyType2D.Kinematic;
            }

        }
 
    }

    private void FixedUpdate()
    {
        if (!CheckPointManager.instance.isPaused)
        {
            Movement();
        }
        else
        {
            if (!CheckPointManager.instance.isDead)
            {
                rb.velocity = Vector2.zero;
               // rb.bodyType = RigidbodyType2D.Kinematic;
            }
        }


    }

    public bool IsGrounded()
    {

        if (Physics2D.OverlapBox(boxCollider.transform.position, boxCollider.size, 0, ground.value))
        {
            return true;
        }
        else
        {
            return false;
        }

           
    }

    public void Movement()
    {
        if (canMove)
        {
            if (rb.velocity.x < 6)
            {
                rb.AddForce(new Vector2(speed, rb.velocity.y));
            }
            else
            {
                //rb.velocity
            }
        }
        

        an.SetBool("IsGrounded", isOnGround);
        an.SetFloat("VelY", rb.velocity.y);

    }

    public void Jump()
    {
        pressTimer -= Time.deltaTime;

        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Ended && rb.velocity.y > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * cutVelY);
                timeJumping = 0;
                pressTimer = pressTime - pressTimeAdd;
                canPlay = true;

            }
            else if (touch.phase == TouchPhase.Ended)
            {
                canPlay = true;
            }




            if (touch.phase == TouchPhase.Began)
            {
                pressTimer = pressTime + pressTimeAdd;
            }

            if (isOnGround && pressTimer > pressTime && rb.velocity.y <= 0.2f)
            {

                rb.velocity = new Vector2(rb.velocity.x, jumpForce);


                if (canPlay == true)
                {
                    canPlay = false;
                    an.SetTrigger("Jump");
                    audioSource.PlayOneShot(jump);
                    Debug.Log("JUMP");

                }

                //rb.velocity = new Vector2(0, jumpForce * Time.deltaTime);
            }

            if (!isOnGround)
            {
                timeJumping += Time.deltaTime;

                if (timeJumping >= maxTimeJumping)
                {
                    rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * cutVelY);
                    timeJumping = 0;
                }
            }
            else
            {
                timeJumping = 0;

            }


        }

        if (rb.velocity.y <= 0 && rb.velocity.y > -maxNegativeY)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * forceDown);
        }
    }

    public IEnumerator AlphaZero()
    {
        //Debug.Log(" Start ALPHA Zero");
        //StopCoroutine(AlphaOne());
        //yield return AlphaOne();

        while (alpha > 0)
        {

            if (tookDamage)
            {
                alpha -= alphaMutiplier * Time.deltaTime;
                //Debug.Log("Coroutine One Running");
            }

            yield return null;
        }

        alpha = 0;

        StartCoroutine(AlphaOne());
    }

    public IEnumerator AlphaOne()
    {
        StopCoroutine(AlphaZero());
        //Debug.Log(" Start ALPHA One");

        while (alpha < 1)
        {
            if (tookDamage)
            {
                alpha += alphaMutiplier * Time.deltaTime;
                //Debug.Log("Coroutine Zero Running");
            }

            yield return null;
        }

        alpha = 1;

        StartCoroutine(AlphaZero());

    }

    public void Pressing()
    {
        isPressing = true;

        Debug.Log("Pressing");
    }

    public void NotPressing()
    {

        isPressing = false;

    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Death"))
        {
            CheckPointManager.instance.ReloadScene(rb);
        }

        if (other.CompareTag("Sopa"))
        {
            lifeScript.RecoverHealth();

            other.GetComponent<SpriteRenderer>().enabled = false;
            other.GetComponent<BoxCollider2D>().enabled = false;

            Destroy(other.gameObject, 2);
        }
    }


    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.tag == "Enemy")
        {
            if (!tookDamage)
            {
                tookDamage = true;
                audioSource.PlayOneShot(hurt);
                StopCoroutine(AlphaOne());
                StopCoroutine(AlphaZero());

                alpha = 1;

                StartCoroutine(AlphaZero());
                if (rb.velocity.y <= 0)
                {
                    rb.AddForce(new Vector2(-5, 2), ForceMode2D.Impulse);
                }

                lifeScript.TakeDamage();
            }

        }


    }

}
