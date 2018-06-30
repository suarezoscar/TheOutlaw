using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public delegate void DeadEventHandler();

public class Player : Character
{
    [SerializeField]
    Camera camara;

    //[SerializeField]
    //private GameObject[] Bullets;

    //private int bulletcount = 5;

    //[SerializeField]
    //private Transform explosionPosition;

    [SerializeField]
    private GameObject playerUlt;

    [SerializeField]
    private AudioClip ultiReadySound;

    bool ultiready=true;

    [SerializeField]
    private AudioSource audio;

    [SerializeField]
    private AudioClip respawnSound;

    [SerializeField]
    private AudioClip walkSound;

    [SerializeField]
    private AudioClip bulletSound;

    [SerializeField]
    private AudioClip jumpSound;

    [SerializeField]
    private AudioClip slideSound;

    [SerializeField]
    private AudioClip coinSound;

    [SerializeField]
    private AudioClip damageSound;

    [SerializeField]
    private AudioClip deadSound;

    [SerializeField]
    private AudioClip bossMusic;

    [SerializeField]
    private AudioClip bossEncounterSound;

    [SerializeField]
    private AudioClip OutOfAmmoSound;

    [SerializeField]
    private AudioClip reloadSound;


    private static Player instance;

    public event DeadEventHandler Dead;

    public bool DeadCollision = false;

    [SerializeField]
    private Stat healthStat;

    [SerializeField]
    private Stat manaStat;

    private int MaxBullets=5;
    public static Player Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<Player>();
            }
            return instance;
        }
    }

    [SerializeField]
    private Transform[] groundPoints;

    [SerializeField]
    private float groundRadius;

    [SerializeField]
    private LayerMask whatIsGround;
    
    [SerializeField]
    private bool airControl;

    [SerializeField]
    private float jumpForce;

    private bool immortal = false;

    private SpriteRenderer spriteRenderer;

    [SerializeField]
    private float immmortalTime;
    
    public Rigidbody2D MyRigidbody { set; get; }

    public bool recarga = false;
    public bool Slide { get; set; }
    public bool Jump { get; set; }
    public bool OnGround { get; set; }

    public override bool IsDead
    {
        get
        {
            if (healthStat.CurrentVal <= 0 && GameManager.Instance.Lives>=0)
            {
                OnDead();
            }
            
            return healthStat.CurrentVal <= 0;
        }
    }

    public int MaxBullets1
    {
        get
        {
            return MaxBullets;
        }

        set
        {
            MaxBullets = value;
        }
    }

    private Vector2 startPos;

    // Use this for initialization
    public override void Start ()
    {
        base.Start();
        startPos = GetComponentInParent<Transform>().position;
        spriteRenderer = GetComponent<SpriteRenderer>();
        MyRigidbody = GetComponent<Rigidbody2D>();
        healthStat.Initialize();
        manaStat.Initialize();
        Player.Instance.BulletsNumber = MaxBullets;
	
	}

    void Update()
    {
        if (GameManager.Instance.Lives < 0 && GameManager.Instance.Restart==false)
        {
            GameManager.Instance.OnGameOver();
        }
        else
        {
            if (!TakingDamage && !IsDead)
            {
                if (DeadCollision && GameManager.Instance.Lives >= 0)
                {
                    Death();
                }
                HandleInput();
            }
        }
        
        
    }

	// Update is called once per frame
	void FixedUpdate ()
    {
        if (GameManager.Instance.Lives < 0)
        {
            GameManager.Instance.OnGameOver();
        }
        else
        {
            if (!TakingDamage && !IsDead)
            {
                if (manaStat.CurrentVal == manaStat.MaxVal && ultiready)
                {
                    audio.PlayOneShot(ultiReadySound);
                    ultiready = false;
                }
                float horizontal = Input.GetAxis("Horizontal");

                OnGround = IsGrounded();

                HandleMovement(horizontal);

                Flip(horizontal);

                HandleLayers();
            }
        }
        
	}

    public void OnDead()
    {
        if (Dead != null)
        {
            Dead();
        }
    }
    
    private void HandleMovement(float horizontal)
    {
        if (MyRigidbody.velocity.y < 0)
        {
            MyAnimator.SetBool("land", true);
        }
        if (!Attack && !Slide && (OnGround || airControl))
        {
            MyRigidbody.velocity = new Vector2(horizontal * movementSpeed, MyRigidbody.velocity.y);
        }
        if (Jump && MyRigidbody.velocity.y == 0)
        {
            MyRigidbody.AddForce(new Vector2(0, jumpForce));
        }

        MyAnimator.SetFloat("speed", Mathf.Abs(horizontal));
        
    }

    
    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MyAnimator.SetTrigger("jump");
            audio.PlayOneShot(jumpSound);
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (Player.Instance.NoAmmo)
            {
                //sin balas hay que recargar
                audio.PlayOneShot(OutOfAmmoSound);
            }
            else
            {
                MyAnimator.SetTrigger("attack");
                Invoke("CaidaBala", 0.75f);
            }
        }
        if (Input.GetKeyDown(KeyCode.R) && OnGround)
        {
            MyAnimator.SetTrigger("reload");
            audio.PlayOneShot(reloadSound);
            //StartCoroutine("Recargar");
        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            MyAnimator.SetTrigger("slide");
            audio.PlayOneShot(walkSound);
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            MyAnimator.SetTrigger("shoot");
            
        }
        if (Input.GetKeyDown(KeyCode.Q) && (manaStat.CurrentVal >= manaStat.MaxVal))
        {
            Debug.Log("Entrando en ulti");
            //Player.Instance.MyRigidbody.velocity = Vector3.zero;
            playerUlt.GetComponent<Animator>().SetTrigger("ulti");
            Invoke("Ulti", 3f);
            manaStat.CurrentVal = 0;
            ultiready = true;

        }
    }

    private void Ulti()
    {
        playerUlt.GetComponent<McreeUlti>();
    }

    private void CaidaBala()
    {
        audio.PlayOneShot(bulletSound);
    }

    private void Flip(float horizontal)
    {
        if (horizontal > 0 && !facingRight || horizontal < 0 && facingRight)
        {
            ChangeDirection();
        }
    }
    
    private bool IsGrounded()
    {
        if (MyRigidbody.velocity.y <= 0)
        {
            foreach (Transform point in groundPoints)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundRadius, whatIsGround);

                for (int i = 0; i < colliders.Length; i++)
                {
                    if (colliders[i].gameObject != gameObject)
                    {
                        return true;
                    }

                }
            }
        }
        return false;

    }
    

    private void HandleLayers()
    {
        if (!OnGround)
        {
            MyAnimator.SetLayerWeight(1, 1);
        }
        else
        {
            MyAnimator.SetLayerWeight(1, 0);
        }
    }
    

    public override void Shoot(int value)
    {
        if (!OnGround && value == 1 || OnGround && value == 0)
        {
            base.Shoot(value);
        }
        
    }

    private IEnumerator IndicateImmortal()
    {
        while (immortal)
        {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(.1f);
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(.1f);
        }
    }

    public override IEnumerator TakeDamage()
    {
        if (!immortal)
        {
            healthStat.CurrentVal -= 10;

            if (!IsDead)
            {
                MyAnimator.SetTrigger("damage");
                audio.PlayOneShot(damageSound);
                immortal = true;
                StartCoroutine(IndicateImmortal());
                yield return new WaitForSeconds(immmortalTime);

                immortal = false;
            }
            else
            {
                GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                GetComponent<Rigidbody2D>().gravityScale = 0;
                GetComponent<BoxCollider2D>().enabled = false;
                MyAnimator.SetLayerWeight(1, 0);
                MyAnimator.SetTrigger("die");
                
                audio.PlayOneShot(deadSound);
            }
        }
    }

    public override void Death()
    {
        DeadCollision = false;
        GetComponent<Rigidbody2D>().gravityScale = 2;
        GetComponent<BoxCollider2D>().enabled = true;
        GameManager.Instance.Lives--;
        MyRigidbody.velocity = Vector2.zero;
        MyAnimator.SetTrigger("idle");
        healthStat.CurrentVal = healthStat.MaxVal;
        transform.position = startPos;
        audio.PlayOneShot(respawnSound);

        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Coin")
        {
            Physics2D.IgnoreCollision(other.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
            GetComponent<AudioSource>().PlayOneShot(coinSound, 0.5f);
            GameManager.Instance.CollectedCoins++;
            GameManager.Instance.Puntuacion += 100;
            manaStat.CurrentVal+=5;
            Destroy(other.gameObject);
        }
        else if (other.gameObject.tag == "Death")
        {
            DeadCollision = true;
        }
        else if (other.gameObject.tag == "BossMusic")
        {
            camara.GetComponent<AudioSource>().Stop();
            camara.GetComponent<AudioSource>().clip = bossMusic;
            camara.GetComponent<AudioSource>().Play();
            audio.PlayOneShot(bossEncounterSound);
            other.gameObject.SetActive(false);
        }
        else if (other.gameObject.tag == "CheckPoint")
        {
            startPos = new Vector2(other.gameObject.GetComponent<Transform>().position.x, other.gameObject.GetComponent<Transform>().position.y);
            other.gameObject.SetActive(false);
        }
        else if(other.gameObject.tag=="Enemy")
        {
            Physics2D.IgnoreCollision(other.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
        else if (other.gameObject.tag == "BossDead")
        {

            SceneManager.LoadScene("End");
        }
    }

    //private IEnumerator Recargar()
    //{
    //    audio.PlayOneShot(reloadSound);

    //    foreach (GameObject item in Player.instance.Bullets)
    //    {
    //        yield return new WaitForSeconds(.4f);
    //        item.SetActive(true);
    //    }
    //    Player.Instance.BulletsNumber = 5;
    //    Player.Instance.NoAmmo = false;
    //}

    //private void OnTriggerEnter2D(CircleCollider2D other)
    //{
    //    if (other.gameObject.tag == "Explosion")
    //    {
    //        Instantiate(explosionPrefab, explosionPosition.position, Quaternion.identity);
    //        Destroy(other.gameObject);
    //    }
    //}
}
