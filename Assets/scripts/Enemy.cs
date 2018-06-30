using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;

public class Enemy : Character
{
    [SerializeField]
    AudioSource audio;

    [SerializeField]
    AudioClip damage;

    [SerializeField]
    AudioClip bossDead;

    [SerializeField]
    GameObject DeadScene;

    private IEnemyState currentState;

    public GameObject Target { set; get; }

    [SerializeField]
    private float meleeRange;

    public bool InMeleeRange
    {
        get
        {
            if (Target != null)
            {
                return Vector2.Distance(transform.position, Target.transform.position) <= meleeRange;
            }
            else
            {
                return false;
            }
        }
    }

    [SerializeField]
    private float shootRange;

    public bool InShootRange
    {
        get
        {
            if (Target != null)
            {
                return Vector2.Distance(transform.position, Target.transform.position) <= shootRange;
            }
            else
            {
                return false;
            }
        }
    }

    public override bool IsDead
    {
        get
        {
            return health <= 0;
        }
    }

    public Transform LeftEdge
    {
        get
        {
            return leftEdge;
        }

        set
        {
            leftEdge = value;
        }
    }

    public Transform RightEdge
    {
        get
        {
            return rightEdge;
        }

        set
        {
            rightEdge = value;
        }
    }

    private Vector3 startPos;

    [SerializeField]
    private Transform leftEdge;
    [SerializeField]
    private Transform rightEdge;

    private bool dropItem=true;

    // Use this for initialization
    public override void Start ()
    {
        base.Start();
        startPos = transform.position;
        if (LeftEdge == null || rightEdge==null)
        {
            LeftEdge.position =new Vector3(startPos.x - 5.47f,startPos.y,0);
            RightEdge.position = new Vector3(startPos.x + 5.47f, startPos.y, 0);
        }
        Player.Instance.Dead += new DeadEventHandler(RemoveTarget);
        ChangeState(new IdleState());
	}

	// Update is called once per frame
	void Update ()
    {
        if (!IsDead)
        {
            if (!TakingDamage)
            {
                currentState.Execute();
            }
            LookAtTarget();
        }
	}

    public void RemoveTarget()
    {
        Target = null;
        ChangeState(new PatrolState());
    }

    private void LookAtTarget()
    {
        if (Target != null)
        {
            float xDir = Target.transform.position.x - transform.position.x;

            if (xDir < 0 && facingRight || xDir > 0 && !facingRight)
            {
                ChangeDirection();
            }
        }
    }

    public void ChangeState(IEnemyState newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }
        currentState = newState;
        currentState.Enter(this);
    }

    public void Move()
    {
        if (!Attack)
        {
            if ((GetDirection().x > 0 && transform.position.x < RightEdge.position.x) 
                || (GetDirection().x < 0 && transform.position.x > LeftEdge.position.x))
            {
                MyAnimator.SetFloat("speed", 1);
                transform.Translate(GetDirection() * (movementSpeed * Time.deltaTime), Space.World);
            }
            else if(currentState is PatrolState)
            {
                ChangeDirection();
            }
            else if (currentState is RangedState)
            {
                Target = null;
                ChangeState(new IdleState());
            }
            
        }

    }

    public Vector2 GetDirection()
    {
        return facingRight ? Vector2.right : Vector2.left;
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        currentState.OnTriggerEnter(other);
    }

    public override IEnumerator TakeDamage()
    {
        health -= 10;
        audio.PlayOneShot(damage);
        if (!IsDead)
        {
            MyAnimator.SetTrigger("damage");
        }
        else
        {
            if (dropItem)
            {
                GameObject coin = (GameObject)Instantiate(GameManager.Instance.CoinPrefab, new Vector3(transform.position.x, transform.position.y + 2), Quaternion.identity);
                Physics2D.IgnoreCollision(coin.GetComponent<Collider2D>(), GetComponent<Collider2D>());
                dropItem = false;
            }
            

            MyAnimator.SetTrigger("die");
            GetComponent<Rigidbody2D>().gravityScale = 0;
            GetComponent<BoxCollider2D>().enabled = false;
            if (name == "Boss")
            {
                audio.PlayOneShot(bossDead);
                DeadScene.SetActive(true);
            }
            yield return null;
        }
    }

    public override void Death()
    {
        dropItem = true;
        Destroy(gameObject);
        
    }


    // OPCIONAL RESPAWN ENEMIGO
    //public override void Death()
    //{
    //    MyAnimator.ResetTrigger("die");
    //    MyAnimator.SetTrigger("damage");
    //    health = 30;
    //    transform.position = startPos;
    //}
}
